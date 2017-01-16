﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataLayer.Entity;
using DocumentsExchange.WebUI.Exceptions;
using DocumentsExchange.WebUI.ViewModels;

namespace DocumentsExchange.WebUI.Controllers
{
    public class RecordT2Controller : BaseController
    {
        private readonly IRecordT2Provider _recordT2Provider;
        private readonly IFileValidator _fileValidator;

        public RecordT2Controller(IRecordT2Provider recordT2Provider, IFileValidator fileValidator)
        {
            _recordT2Provider = recordT2Provider;
            _fileValidator = fileValidator;
        }

        public PartialViewResult Index(int id)
        {
            return PartialView(CreateRecordT2Vm(id));
        }

        public PartialViewResult Create(int orgId)
        {
            var record = new RecordT2()
            {
                OranizationId = orgId,
                CreatedDateTime = DateTime.UtcNow
            };
            return PartialView(record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RecordT2 record)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingRecord =
                        _recordT2Provider.Find(new SearchParams()
                        {
                            NumberPaymentOrder = record.NumberPaymentOrder,
                            OrganizationSender = record.OrganizationSender
                        }).Result;

                    if (existingRecord != null && DateTime.UtcNow < existingRecord.CreatedDateTime.AddMonths(3))
                    {
                        ModelState.AddModelError("record", $"Record with specified data already exists");
                        throw new ValidationException(ModelState);
                    }

                    if (Request.Files != null && Request.Files.Count > 0)
                    {
                        var upload = Request.Files[0];
                        bool result;
                        if (upload != null)
                        {
                            if (!_fileValidator.Validate(upload.FileName))
                            {
                                ModelState.AddModelError("file", $"Unsupported file {upload.FileName}");
                                throw new ValidationException(ModelState);
                            }

                            var stream = upload.InputStream;
                            // and optionally write the file to disk
                            var fileName = Path.GetFileName(upload.FileName);
                            var newFileName = $"{Guid.NewGuid().ToString("n")}{Path.GetExtension(fileName)}";
                            var filePath = new FilePath();

                            var path = Path.Combine(Server.MapPath("~/App_Data/PaymentOrders"), newFileName);
                            using (var fileStream = System.IO.File.Create(path))
                            {
                                stream.CopyTo(fileStream);
                            }

                            filePath.FileName = newFileName;
                            filePath.OriginalFileName = fileName;

                            var now = DateTime.UtcNow;
                            var r = new RecordT2()
                            {
                                CreatedDateTime = now,
                                NumberPaymentOrder = record.NumberPaymentOrder,
                                OrganizationSender = record.OrganizationSender,
                                OrganizationReceiver = record.OrganizationReceiver,
                                Amount = record.Amount,
                                Percent = record.Percent,
                                SenderUserId = UserId,
                                File = filePath,
                                OranizationId = record.OranizationId,

                                Log = new Log()
                                {
                                    UserId = UserId,
                                    CreatedDateTime = now
                                }
                            };
                             result = _recordT2Provider.Add(r).Result;
                        }
                    }
                }
                else
                {
                    return PartialView();
                }
            }

            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Невозможно сохранить изменения. Попробуйте позже.");
                return PartialView();
            }

            return PartialView("Index", CreateRecordT2Vm(record.OranizationId));
        }

        public FileResult DownloadPaymentOrder(string fileName, string originalFileName)
        {
            var path = Path.Combine(Server.MapPath("~/App_Data/PaymentOrders"), fileName);
            byte[] fileContent = System.IO.File.ReadAllBytes(path);
            return File(fileContent, System.Net.Mime.MediaTypeNames.Application.Octet, originalFileName);
        }

        [HttpPost]
        public JsonResult ValidateFiles(string[] fileNames)
        {
            return Json(new
            {
                ValidationResult = fileNames.Select(x => new
                {
                    FileName = x,
                    Valid = _fileValidator.Validate(x)
                }).ToArray()
            }, JsonRequestBehavior.DenyGet);
        }

        public async Task<PartialViewResult> Edit(int recordId)
        {
            return PartialView(await _recordT2Provider.Get(recordId));
        }


        private RecordsT2ViewModel CreateRecordT2Vm(int orgId)
        {
            var records = _recordT2Provider.GetAll(orgId).Result;

            var recordsVm = new RecordsT2ViewModel();

            if (records != null) recordsVm.Records = new List<RecordT2>(records);
            return recordsVm;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = nameof(RecordT2.SenderUser))]RecordT2 record)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingRecord =
                        _recordT2Provider.Find(new SearchParams()
                        {
                            NumberPaymentOrder = record.NumberPaymentOrder,
                            OrganizationSender = record.OrganizationSender
                        }).Result;

                    if (existingRecord != null && existingRecord.Id != record.Id && DateTime.UtcNow < existingRecord.CreatedDateTime.AddMonths(3))
                    {
                        ModelState.AddModelError("record", $"Record with specified data already exists");
                        throw new ValidationException(ModelState);
                    }

                    if (Request.Files != null && Request.Files.Count > 0)
                    {
                        var upload = Request.Files[0];

                        if (upload != null)
                        {
                            if (!_fileValidator.Validate(upload.FileName))
                            {
                                ModelState.AddModelError("file", $"Unsupported file {upload.FileName}");
                                throw new ValidationException(ModelState);
                            }

                            var stream = upload.InputStream;

                            // and optionally write the file to disk
                            var fileName = Path.GetFileName(upload.FileName);
                            var newFileName = $"{Guid.NewGuid().ToString("n")}{Path.GetExtension(fileName)}";

                            // ReSharper disable once AssignNullToNotNullAttribute
                            var path = Path.Combine(Server.MapPath("~/App_Data/PaymentOrders"), newFileName);

                            using (var fileStream = System.IO.File.Create(path))
                            {
                                stream.CopyTo(fileStream);
                            }

                            record.File.OriginalFileName = fileName;
                            record.File.FileName = fileName;
                        }
                    }

                    if (!_recordT2Provider.Update(record).Result)
                    {
                        ModelState.AddModelError("model", "Something went wrong");
                        throw new ValidationException(ModelState);
                    }
                }
                else
                {
                    return PartialView();
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Невозможно сохранить изменения. Попробуйте позже.");
            }

            return PartialView("Index", CreateRecordT2Vm(record.OranizationId));
        }

    }
}
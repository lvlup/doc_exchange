using System;
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
    public class RecordT1Controller : BaseController
    {
        private readonly IRecordT1Provider _recordT1Provider;
        private readonly IFileValidator _fileValidator;
        private readonly IGetCurrencyCourse _getCurrencyCourse;

        public RecordT1Controller(IRecordT1Provider recordT1Provider, IFileValidator fileValidator, IGetCurrencyCourse getCurrencyCourse)
        {
            _recordT1Provider = recordT1Provider;
            _fileValidator = fileValidator;
            _getCurrencyCourse = getCurrencyCourse;
        }


        public PartialViewResult Index(int id)
        {
            return PartialView(CreateRecordT1Vm(id));
        }

        public PartialViewResult Create(int orgId)
        {
            var record = new RecordT1()
            {
                OranizationId = orgId,
                CreatedDateTime = DateTime.UtcNow
            };

            return PartialView(record);
        }

        [HttpPost]
        public JsonResult ValidateFiles(string[] fileNames)
        {
            return Json(new
            {
                ValidationResult = fileNames.Select(x => new
                {
                    FileName = x, Valid = _fileValidator.Validate(x)
                }).ToArray()
            }, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RecordT1 record)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingRecord =
                        _recordT1Provider.Find(new SearchParams()
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
                            // ReSharper disable once AssignNullToNotNullAttribute
                            var path = Path.Combine(Server.MapPath("~/App_Data/PaymentOrders"), newFileName);

                            using (var fileStream = System.IO.File.Create(path))
                            {
                                stream.CopyTo(fileStream);
                            }

                            filePath.OriginalFileName = fileName;
                            filePath.FileName = newFileName;

                            var now = DateTime.UtcNow;

                            var sent = (record.Amount - record.Percent)/record.Course - record.Swift;
                            var amountInCurrency = ((sent + record.Swift) - record.Percent)*record.Course;
                            var r = new RecordT1()
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
                                Course = record.Course,
                                Swift = record.Swift,

                                Sent = sent,
                                AmountInCurrency = amountInCurrency,
                                Total = record.Amount - amountInCurrency,

                                Log = new Log()
                                {
                                    UserId = UserId,
                                    CreatedDateTime = now
                                }
                            };

                            result = _recordT1Provider.Add(r).Result;
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
            }

            return PartialView("Index", CreateRecordT1Vm(record.OranizationId));
        }

        public FileResult DownloadPaymentOrder(string fileName, string originalFileName)
        {
            var path = Path.Combine(Server.MapPath("~/App_Data/PaymentOrders"), fileName);
            byte[] fileContent = System.IO.File.ReadAllBytes(path);
            return File(fileContent, System.Net.Mime.MediaTypeNames.Application.Octet, originalFileName);
        }

        public decimal GetCurrency(string currency)
        {
            Currency currencyCode;
            if (!Enum.TryParse(currency, out currencyCode))
                throw new Exception($"Invalid currency {currency}");

           return _getCurrencyCourse.GetCurrencyByCode(currencyCode);
        }

        private RecordsT1ViewModel CreateRecordT1Vm(int orgId)
        {
            var records = _recordT1Provider.GetAll(orgId).Result;

            var recordsVm = new RecordsT1ViewModel();

            if (records != null) recordsVm.Records = new List<RecordT1>(records);
            return recordsVm;
        }

        public async Task<PartialViewResult> Edit(int recordId)
        {
            return PartialView(await _recordT1Provider.Get(recordId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = nameof(RecordT1.SenderUser))]RecordT1 record)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingRecord =
                        _recordT1Provider.Find(new SearchParams()
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

                    if (!_recordT1Provider.Update(record).Result)
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

            return PartialView("Index", CreateRecordT1Vm(record.OranizationId));
        }
    }
}
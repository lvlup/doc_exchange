using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataLayer.Entity;
using DocumentsExchange.WebUI.ViewModels;

namespace DocumentsExchange.WebUI.Controllers
{
    public class RecordT2Controller : Controller
    {
        private readonly IRecordT2Provider _recordT2Provider;
        private readonly IFilePathProvider _filePathProvider;

        public RecordT2Controller(IRecordT2Provider recordT2Provider, IFilePathProvider iFilePathProvider)
        {
            _recordT2Provider = recordT2Provider;
            _filePathProvider = iFilePathProvider;
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
                CreatedDateTime = DateTime.Now
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
                    if (Request.Files != null && Request.Files.Count > 0)
                    {
                        var upload = Request.Files[0];
                        bool result;
                        if (upload != null)
                        {
                            var stream = upload.InputStream;
                            // and optionally write the file to disk
                            var fileName = Path.GetFileName(upload.FileName);
                            var filePath = new FilePath();
                            if (fileName != null)
                            {
                                var path = Path.Combine(Server.MapPath("~/App_Data/PaymentOrders"), fileName);
                                using (var fileStream = System.IO.File.Create(path))
                                {
                                    stream.CopyTo(fileStream);
                                }
                                filePath.FileName = fileName;
                                //result = _filePathProvider.Add(filePath).Result;
                            }

                            var r = new RecordT2()
                            {
                                CreatedDateTime = record.CreatedDateTime,
                                NumberPaymentOrder = record.NumberPaymentOrder,
                                OrganizationSender = record.OrganizationSender,
                                OrganizationReceiver = record.OrganizationReceiver,
                                Amount = record.Amount,
                                Percent = record.Percent,
                                SenderUser = new User() { FirstName = "user record2"},
                                File = filePath,
                                OranizationId = record.OranizationId
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

        public FileResult DownloadPaymentOrder(int id)
        {
            var filepath = _filePathProvider.Get(id).Result;

            var path = Path.Combine(Server.MapPath("~/App_Data/PaymentOrders"), filepath.FileName);
            byte[] fileContent = System.IO.File.ReadAllBytes(path);

            return File(fileContent, System.Net.Mime.MediaTypeNames.Application.Octet, filepath.FileName);
        }

        private RecordsT2ViewModel CreateRecordT2Vm(int orgId)
        {
            var records = _recordT2Provider.GetAll(orgId).Result;

            var recordsVm = new RecordsT2ViewModel();

            if (records != null) recordsVm.Records = new List<RecordT2>(records);
            return recordsVm;
        }
    }
}
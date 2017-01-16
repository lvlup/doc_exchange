using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataLayer.Entity;
using DocumentsExchange.WebUI.ViewModels;

namespace DocumentsExchange.WebUI.Controllers
{
    public class RecordT1Controller : Controller
    {
        private readonly IRecordT1Provider _recordT1Provider;
        private readonly IFilePathProvider _filePathProvider;
        private readonly IGetCurrencyCourse _getCurrencyCourse;

        public RecordT1Controller(IRecordT1Provider recordT1Provider, IFilePathProvider filePathProvider, IGetCurrencyCourse getCurrencyCourse)
        {
            _recordT1Provider = recordT1Provider;
            _filePathProvider = filePathProvider;
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
                CreatedDateTime = DateTime.Now
            };

            return PartialView(record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RecordT1 record)
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

                            var r = new RecordT1()
                            {
                                CreatedDateTime = record.CreatedDateTime,
                                NumberPaymentOrder = record.NumberPaymentOrder,
                                OrganizationSender = record.OrganizationSender,
                                OrganizationReceiver = record.OrganizationReceiver,
                                Amount = record.Amount,
                                Percent = record.Percent,
                                SenderUser = new User() { FirstName = "user record2" },
                                File = filePath,
                                OranizationId = record.OranizationId
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

        public FileResult DownloadPaymentOrder(int id)
        {
            var filepath = _filePathProvider.Get(id).Result;

            var path = Path.Combine(Server.MapPath("~/App_Data/PaymentOrders"), filepath.FileName);
            byte[] fileContent = System.IO.File.ReadAllBytes(path);

            return File(fileContent, System.Net.Mime.MediaTypeNames.Application.Octet,filepath.FileName);
        }

        public decimal GetCurrency(string currency)
        {
           return _getCurrencyCourse.GetCurrencyByCode(Currency.Usd);
        }

        private RecordsT1ViewModel CreateRecordT1Vm(int orgId)
        {
            var records = _recordT1Provider.GetAll(orgId).Result;

            var recordsVm = new RecordsT1ViewModel();

            if (records != null) recordsVm.Records = new List<RecordT1>(records);
            return recordsVm;
        }
    }
}
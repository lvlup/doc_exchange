using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Identity;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataLayer.Entity;
using DocumentsExchange.WebUI.Exceptions;
using DocumentsExchange.WebUI.ViewModels;
using File = DocumentsExchange.DataLayer.Entity.File;

namespace DocumentsExchange.WebUI.Controllers
{
    [Authorize]
    public class FileController : BaseController
    {
        private readonly IFileProvider _fileProvider;
        private readonly IFileCategoryProvider _fileCategoryProvider;
        private readonly IFileValidator _fileValidator;

        public FileController(IFileProvider fileProvider, IFileCategoryProvider fileCategoryProvider, IFileValidator fileValidator)
        {
            _fileProvider = fileProvider;
            _fileCategoryProvider = fileCategoryProvider;
            _fileValidator = fileValidator;
        }


        public PartialViewResult Index(int orgId,int categoryId)
        {
            var category = _fileCategoryProvider.Get(categoryId).Result;

            var fileVm = new FileViewModel() {FileCategory = category,OrganizationId = orgId};

            return PartialView(fileVm);
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.Technician)]
        public PartialViewResult Create(int orgId, int categoryId)
        {
            File file = new File() { CategoryId = categoryId,OranizationId = orgId};
            return PartialView(file);
        }

        public PartialViewResult FilesTable(int orgId, int categoryId)
        {
            return PartialView(CreateFileVm(orgId,categoryId));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Admin + "," + Roles.Technician)]
        public  ActionResult Create(File file)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new ValidationException(ModelState);

                    if (Request.Files != null && Request.Files.Count > 0)
                    {
                        bool result = false;
                        var upload = Request.Files[0];
                        if (upload != null)
                        {
                            var doc = new File
                            {
                                FileName = file.FileName,
                                AddedDateTime = DateTime.UtcNow,
                                FileType = Path.GetExtension(upload.FileName)?.Replace(".", string.Empty).ToLower(),
                                OranizationId = file.OranizationId,
                                CategoryId = file.CategoryId,
                                ContentType = upload.ContentType,
                                UserId = UserId
                            };
                            using (var reader = new System.IO.BinaryReader(upload.InputStream))
                            {
                                doc.Content = reader.ReadBytes(upload.ContentLength);
                            }
                            result = _fileProvider.Add(doc).Result;
                        }
                    return Json(new { Success = result });
                }

                ModelState.AddModelError("file", $"No files provided");
                throw new ValidationException(ModelState);
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Невозможно сохранить изменения. Попробуйте позже.");
                return Json(new { Success = false });
            }

            //catch (RetryLimitExceededException)
            //{
            //    ModelState.AddModelError("", "Невозможно сохранить изменения. Попробуйте позже.");
            //}

            //return PartialView("FilesTable", CreateFileVm(file.OranizationId, file.CategoryId));
        }

        public FileResult DownloadFile(int id)
        {
            var file = _fileProvider.Get(id).Result;
            return File(file.Content, System.Net.Mime.MediaTypeNames.Application.Octet, file.FileName + "." + file.FileType);
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


        private FileViewModel CreateFileVm(int orgId,int categoryId)
        {
            var fileVm = new FileViewModel() { OrganizationId = orgId };

            var files = _fileProvider.GetFilesWithCategory(orgId, categoryId).Result;
            if (files != null) fileVm.Files = new List<File>(files);
            return fileVm;
        }
    }
}
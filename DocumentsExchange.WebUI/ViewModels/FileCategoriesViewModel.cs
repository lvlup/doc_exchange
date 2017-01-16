using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.WebUI.ViewModels
{
    public class FileCategoriesViewModel
    {
        public int OrganizationId { get; set; }

        public  ICollection<FileCategory> FileCategories { get; set; }
    }
}
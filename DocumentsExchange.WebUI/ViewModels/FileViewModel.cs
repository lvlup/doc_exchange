using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.WebUI.ViewModels
{
    public class FileViewModel
    {
        public ICollection<File> Files { get; set; }

        public static ICollection<string> PropertyNames { get; set; }

        public FileCategory FileCategory { get; set; }

        public int OrganizationId { get; set; }

        static FileViewModel()
        {
            PropertyNames = new List<string>();

            var properties = typeof(File).GetProperties();
            foreach (var prop in properties)
            {
                var attr = prop.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                    .Cast<DisplayNameAttribute>().FirstOrDefault();
                if (attr != null)
                {
                    string displayName = attr.DisplayName;
                    PropertyNames.Add(displayName);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.WebUI.ViewModels
{
    public class RecordsT1ViewModel
    {
        public ICollection<RecordT1> Records { get; set; }

        public int OrgId { get; set; }

        public static  ICollection<string> PropertyNames { get; set; } 

        static RecordsT1ViewModel()
        {
            PropertyNames  = new List<string>();

            var properties = typeof(RecordT1).GetProperties();
            foreach (var prop in properties)
            {
                var attr = prop.GetCustomAttributes(typeof (DisplayNameAttribute), true)
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
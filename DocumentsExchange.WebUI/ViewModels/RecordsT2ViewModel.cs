using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.WebUI.ViewModels
{
    public class RecordsT2ViewModel
    {
        public int OrgId { get; set; }

        public ICollection<RecordT2> Records { get; set; }

        public static ICollection<string> PropertyNames { get; set; }

        static RecordsT2ViewModel()
        {
            PropertyNames = new List<string>();

            var properties = typeof(RecordT2).GetProperties();
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
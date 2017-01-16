using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.WebUI.ViewModels
{
    public class UserViewModel
    {
        public ICollection<User> Users { get; set; }

        public static ICollection<string> PropertyNames { get; set; }

        static UserViewModel()
        {
            PropertyNames = new List<string>();

            var properties = typeof(User).GetProperties();
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
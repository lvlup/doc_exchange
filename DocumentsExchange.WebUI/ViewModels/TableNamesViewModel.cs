using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentsExchange.WebUI.ViewModels
{
    public class TableNamesViewModel
    {
        public List<SelectListItem> TableList { get; set; }

        public int OrganizationId { get; set; }

        public TableNamesViewModel()
        {
            var names = new List<string>()
            {
                "Таблица 1",
                "Таблица 2"
            };
            TableList = TableList = new List<SelectListItem>()
                {
                    new SelectListItem() {Selected = true,Text = names[0],Value = "Table1"},
                    new SelectListItem() {Text = names[1],Value = "Table2"},
                };
        }
    }
}
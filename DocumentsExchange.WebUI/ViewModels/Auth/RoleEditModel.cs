using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DocumentsExchange.DataLayer.Entity;
using DocumentsExchange.DataLayer.Identity;

namespace DocumentsExchange.WebUI.ViewModels.Auth
{
    public class RoleEditModel
    {
        public AppRole Role { get; set; }
        public IEnumerable<User> Members { get; set; }
        public IEnumerable<User> NonMembers { get; set; }
    }
}
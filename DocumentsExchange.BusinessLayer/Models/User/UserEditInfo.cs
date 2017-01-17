using System.Collections.Generic;

namespace DocumentsExchange.BusinessLayer.Models.User
{
    public class UserEditInfo
    {
        public DataLayer.Entity.User User { get; set; }

        public IEnumerable<OrganizationInfo> Organizations { get; set; }
    }
}

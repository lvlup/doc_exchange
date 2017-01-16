using Microsoft.AspNet.Identity.EntityFramework;

namespace DocumentsExchange.DataLayer.Identity
{
    public class AppRole : IdentityRole<int, AppUserRole>
    {
        public AppRole()
        {
            
        }

        public AppRole(string roleName)
        {
            Name = roleName;
        }
    }
}

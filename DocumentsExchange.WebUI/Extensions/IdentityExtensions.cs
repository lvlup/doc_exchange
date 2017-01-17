using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNet.Identity;

namespace DocumentsExchange.WebUI.Extensions
{
    public static class CustomIdentityExtensions
    {
        public static string GetFullName(this IIdentity identity)
        {
            return (identity as ClaimsIdentity).FindFirstValue(ClaimTypes.GivenName);
        }
    }
}
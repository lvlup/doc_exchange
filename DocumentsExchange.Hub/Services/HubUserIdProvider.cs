using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

namespace DocumentsExchange.Hub.Services
{
    class HubUserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            return request.User.Identity.GetUserId<int>().ToString();
        }
    }
}

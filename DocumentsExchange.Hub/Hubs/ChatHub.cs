using System;
using System.Linq;
using System.Threading.Tasks;
using DocumentsExchange.BusinessLayer.Models;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.Hub.Models;
using DocumentsExchange.Hub.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace DocumentsExchange.Hub.Hubs
{
    [Authorize]
    [HubName("chathub")]
    public class ChatHub : Microsoft.AspNet.SignalR.Hub
    {
        private readonly ConnectionPool<HubUser, string> _connectionPool;
        private readonly IMessagesProvider _messagesProvider;
        private readonly IUserIdProvider _userIdProvider;

        public ChatHub(
            ConnectionPool<HubUser, string> connectionPool, 
            IMessagesProvider messagesProvider,
            IUserIdProvider userIdProvider)
        {
            _connectionPool = connectionPool;
            _messagesProvider = messagesProvider;
            _userIdProvider = userIdProvider;
        }
        
        public async Task SendMessage(dynamic message)
        {
            string newId = Guid.NewGuid().ToString("n");
            HubMessage newMessage = new HubMessage()
            {
                Content = message.content,
                Id = newId,
                UserId = message.userId,
                OrganizationId = message.organizationId,
                TimeStamp = message.timeStamp
            };

            if (await _messagesProvider.AddMessage(newMessage))
            {
                var relatedUsers = await _messagesProvider.GetRelatedUsers(newMessage.UserId, newMessage.OrganizationId);
                await
                    Task.WhenAll(
                        Clients.Users(relatedUsers.Select(x => x.ToString()).ToList()).onMessageReceived(message),
                        Clients.Caller.messageSent(new
                        {
                            id = message.id,
                            newId = newId
                        }));

                return;
            }

            throw new HubException("Message {0} wasn't sent", message);
        }

        public async Task Ping()
        {
            await Clients.Caller.pingBack();
        }

        public override async Task OnConnected()
        {
            GetOrRegister();
            //await NotifyRelatedUsers(_userIdProvider.GetUserId(Context.Request), UserState.Online);
            await base.OnConnected();
        }

        public override async Task OnReconnected()
        {
            GetOrRegister();
            //await NotifyRelatedUsers(_userIdProvider.GetUserId(Context.Request), UserState.Online);
            await base.OnReconnected();
        }

        public override async Task OnDisconnected(bool stopCalled)
        {
            string userId = _userIdProvider.GetUserId(Context.Request);

            var mapping = _connectionPool.Remove(userId, Context.ConnectionId);
            
            if (mapping != null && mapping.Connections.Count == 0)
            {
                // user disconnected
                //await NotifyRelatedUsers(mapping.User.Id, UserState.Offline);
            }

            await base.OnDisconnected(stopCalled);
        }

        private HubUser GetOrRegister()
        {
            if (!Context.User.Identity.IsAuthenticated)
            {
                return null;
            }

            string userId = _userIdProvider.GetUserId(Context.Request);
            var existedUser = _connectionPool.TryGetUser(u => u.Id == userId);

            if (existedUser == null)
            {
                existedUser = new HubUser()
                {
                    Id = userId,
                    State = UserState.Online
                };
            }

            _connectionPool.TryAdd(existedUser, Context.ConnectionId);

            return existedUser;
        }

        //private async Task NotifyRelatedUsers(string userId, UserState state)
        //{
        //    var relatedUsers = await _messagesProvider.GetRelatedUsers(userId.ToInt());
        //    if (relatedUsers != null)
        //        await Clients.Users(relatedUsers.Select(x => x.ToString()).ToList()).userStateChanged(new
        //        {
        //            Id = userId,
        //            State = (int) state,
        //            //isMobile = mapping.User.IsMobile
        //        });
        //}
    }
}

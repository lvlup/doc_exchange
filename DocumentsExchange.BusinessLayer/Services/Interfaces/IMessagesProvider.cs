using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentsExchange.BusinessLayer.Models;
using DocumentsExchange.Common.Extensions;
using DocumentsExchange.DataAccessLayer.Models;
using DocumentsExchange.DataAccessLayer.Repository;
using DocumentsExchange.DataLayer.Entity;
using DocumentsExchange.Models;

namespace DocumentsExchange.BusinessLayer.Services.Interfaces
{
    public interface IMessagesProvider
    {
        Task<bool> AddMessage(HubMessage message);

        Task<ItemsResult<Message>> GetMessages(int orgId, PageInfo paging);

        Task<IEnumerable<int>> GetRelatedUsers(int userId, int orgId);
    }

    class MessagesProvider : IMessagesProvider
    {
        private readonly UserRepository _userRepository;
        private readonly MessageRepository _messageRepository;

        public MessagesProvider(UserRepository userRepository, MessageRepository messageRepository)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
        }

        public async Task<bool> AddMessage(HubMessage message)
        {
            Message dbMessage = new Message()
            {
                Content = message.Content,
                Id = message.Id,
                OrganizationId = message.OrganizationId,
                SenderId = message.UserId,
                TimeStamp = message.TimeStamp.Convert()
            };


            return await _messageRepository.Create(dbMessage);
        }

        public async Task<ItemsResult<Message>> GetMessages(int orgId, PageInfo paging)
        {
            return await _messageRepository.Get(orgId, paging);
        }

        public async Task<IEnumerable<int>> GetRelatedUsers(int userId, int orgId)
        {
            var users = await _userRepository.GetAll();
            return users.Select(x => x.Id).Except(new [] { userId });

            // todo
            //var users = await _userRepository.GetAll(orgId, userId);
            //return users.Select(x => x.Id);
        }
    }
}

using System.Threading.Tasks;
using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataAccessLayer.Models;

namespace DocumentsExchange.WebUI.Controllers
{
    [Authorize]
    public class MessagesController : BaseController
    {
        private readonly IMessagesProvider _messagesProvider;

        public MessagesController(IMessagesProvider messagesProvider)
        {
            _messagesProvider = messagesProvider;
        }

        public async Task<JsonResult> GetMessages(int orgId, PageInfo paging)
        {
            return Json(await _messagesProvider.GetMessages(orgId, paging), JsonRequestBehavior.AllowGet);
        }
    }
}
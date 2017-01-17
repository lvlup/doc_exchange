using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsExchange.BusinessLayer.Models
{
    public class HubMessage
    {
        public int UserId { get; set; }

        public int OrganizationId { get; set; }

        public string Content { get; set; }

        public long TimeStamp { get; set; }

        public string Id { get; set; }
    }
}

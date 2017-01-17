using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentsExchange.DataLayer.Entity
{
    public class Message
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public DateTime TimeStamp { get; set; }

        public virtual Organization Organization { get; set; }

        public int OrganizationId { get; set; }

        public int SenderId { get; set; }

        public virtual User Sender { get; set; }

        [NotMapped]
        public string UserName { get; set; }
    }
}

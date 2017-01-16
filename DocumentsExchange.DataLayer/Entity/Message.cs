using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsExchange.DataLayer.Entity
{
   public class Message
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public DateTime TimeStamp { get; set; }

        public int SenderId { get; set; }

        public virtual User Sender { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsExchange.DataLayer.Entity
{
   public class Change
    {
        public int Id { get; set; }

        public string PropertyName { get; set; }

        public DateTime TimeSpan { get; set; }

        public string CurrentValue { get; set; }

        public string OldValue { get; set; }
    }
}

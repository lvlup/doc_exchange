using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsExchange.DataLayer.Entity
{
   public class Log
    {
        public int Id { get; set; }

        public virtual  ICollection<Change> Changes { get; set; } 
    }
}

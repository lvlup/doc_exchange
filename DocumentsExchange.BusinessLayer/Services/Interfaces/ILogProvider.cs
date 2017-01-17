using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.BusinessLayer.Services.Interfaces
{
   public interface ILogProvider
    {
        Task<Log> Get(int id);
    }
}

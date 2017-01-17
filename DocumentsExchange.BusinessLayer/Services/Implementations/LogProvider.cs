using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataAccessLayer.Repository;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.BusinessLayer.Services.Implementations
{
    public class LogProvider : ILogProvider
    {

        private readonly LogRepository _logRepository;

        public LogProvider(LogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<Log> Get(int id)
        {
            return await _logRepository.Get(id).ConfigureAwait(false);
        }
    }
}

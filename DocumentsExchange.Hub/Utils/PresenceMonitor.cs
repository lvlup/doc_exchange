using Autofac.Extras.NLog;

namespace DocumentsExchange.Hub.Utils
{
    public class PresenceMonitor
    {
        private readonly ILogger _logger;

        public PresenceMonitor(ILogger logger)
        {
            _logger = logger;
        }

        public void Start()
        {
            
        }
    }
}

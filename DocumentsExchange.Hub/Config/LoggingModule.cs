using System.Runtime.CompilerServices;
using Autofac.Extras.NLog;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace DocumentsExchange.Hub.Config
{
    public class LoggingModule : HubPipelineModule
    {
        private readonly ILogger _logger;

        public LoggingModule(ILogger logger)
        {
            _logger = logger;
        }

        protected override bool OnBeforeAuthorizeConnect(HubDescriptor hubDescriptor, IRequest request)
        {
            _logger.Trace(request.Headers);
            _logger.Trace(request.User);
            bool res = base.OnBeforeAuthorizeConnect(hubDescriptor, request);
            _logger.Trace(res);
            return res;
        }

        protected override void OnAfterConnect(IHub hub)
        {
            LogInfo(hub);

            base.OnAfterConnect(hub);
        }


        protected override void OnAfterDisconnect(IHub hub, bool stopCalled)
        {
            LogInfo(hub);

            base.OnAfterDisconnect(hub, stopCalled);
        }

        protected override void OnAfterReconnect(IHub hub)
        {
            LogInfo(hub);

            base.OnAfterReconnect(hub);
        }

        protected override bool OnBeforeConnect(IHub hub)
        {
            LogInfo(hub);

            return base.OnBeforeConnect(hub);
        }

        protected override bool OnBeforeDisconnect(IHub hub, bool stopCalled)
        {
            LogInfo(hub);

            return base.OnBeforeDisconnect(hub, stopCalled);
        }

        protected override bool OnBeforeReconnect(IHub hub)
        {
            LogInfo(hub);

            return base.OnBeforeReconnect(hub);
        }

        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            _logger.Error("Error occured");
            _logger.Error(exceptionContext.Error);
            _logger.Error(exceptionContext.Result);

            base.OnIncomingError(exceptionContext, invokerContext);
        }

        private void LogInfo(IHub hub, [CallerMemberName] string methodName = "")
        {
            _logger.Trace("{3}:: User:{0}; Authenticated:{1}; AuthType:{2}",
                hub.Context.User?.Identity?.Name,
                hub.Context.User?.Identity?.IsAuthenticated,
                hub.Context.User?.Identity?.AuthenticationType,
                methodName);
        }
    }
}

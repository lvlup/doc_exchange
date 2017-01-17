(function (window, SignalR, SiteActivityListener) {
    SiteActivityListener.start = function (options) {
        SignalR.Hub.onSiteStopped().add(function () {
            window.location.href = options.redirectUrl;
        });

        SignalR.Hub.start();
    };
})(window, SignalR, SiteActivityListener = {})
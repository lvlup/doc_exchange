(function (SignalR) {
    'use strict';

    var PING_INTERVAL = 60000,
        RECONNECT_INTERVAL = 5000,
        callsQueue = [];

    var connectedHandlers = [];

    var EventHandler = function () {
        this.counter = 0;
        this.handlers = [];
    };

    EventHandler.prototype.add = function (handler) {
        var id = ++this.counter;
        this.handlers.push({
            handler: handler,
            id: id
        });

        return id;
    };

    EventHandler.prototype.remove = function (handlerId) {
        var handlers = this.handlers.filter(function (h) { return h.id === handlerId; });
        if (handlers.length)
            this.handlers.splice(this.handlers.indexOf(handlers[0]), 1);
    };

    EventHandler.prototype.clear = function() {
        this.handlers.length = 0;
    };

    var eventHandlers = {
        pingBack: new EventHandler(),
        onMessageReceived: new EventHandler(),
        messageSent: new EventHandler(),
        userStateChanged: new EventHandler(),
        onSiteStopped: new EventHandler()
    };

    var currentState,
        pingIntervalId,
        tryingToReconnect = false;

    // setup
    var connection = $.hubConnection('/signalr', { useDefaultPath: false });
    var hub = connection.createHubProxy('chathub');

    //$.connection.transports.longPolling.supportsKeepAlive = function () {
    //    return false;
    //}

    hub.logging = true;

    var getStateString = function (state) {
        switch (state) {
            case $.signalR.connectionState.connected:
                return "Connected";
            case $.signalR.connectionState.connecting:
                return "Connecting...";
            case $.signalR.connectionState.disconnected:
                return "Disconnected";
            case $.signalR.connectionState.reconnecting:
                return "Reconnecting...";
        }

        return "UNKNOWN [" + state + "]";
    };

    var queueOrCall = function (serverCall) {
        if (currentState !== $.signalR.connectionState.connected) {
            callsQueue.push(serverCall);

            return;
        }

        serverCall();
    }

    //start the connection
    var start = function () {
        if (currentState === $.signalR.connectionState.connected) {
            console.log('already connected');
            return;
        }

        connection.start({ waitForPageLoad: false })
            .done(function () {
                for (let i = 0; i < connectedHandlers.length; i++) {
                    connectedHandlers[i]();
                }

                while (callsQueue.length) {
                    var call = callsQueue.shift();
                    call();
                }

                if (pingIntervalId)
                    clearInterval(pingIntervalId);

                pingIntervalId = setInterval(function () {
                    if (connection.state !== $.signalR.connectionState.connected)
                        return;

                    hub.invoke('Ping');
                }, PING_INTERVAL);
            }).fail(function () {
                console.log("[" + new Date() + "] SignalR: Failed to connect to the server");
            });
    };

    connection.error(function (error) {
        console.log("[" + new Date() + "] SignalR: Error occured - " + error);
    });

    connection.reconnecting(function () {
        console.log("[" + new Date() + "] SignalR: Reconnecting...");
        tryingToReconnect = true;
    });

    connection.reconnected(function () {
        console.log("[" + new Date() + "] SignalR: Reconnected...");
        tryingToReconnect = false;
    });

    connection.stateChanged(function (state) {
        currentState = state.newState;

        console.log("[" +
            new Date() +
            "] SignalR: State Changed " +
            getStateString(state.oldState) +
            " --> " +
            getStateString(state.newState));
    });

    connection.disconnected(function () {
        if (connection.lastError) {
            console.error("[" + new Date() + "] SignalR: Error: " + connection.lastError.message);
        }

        if (!tryingToReconnect) {
            if (pingIntervalId) {
                clearInterval(pingIntervalId);
                pingIntervalId = null;
            }

            setTimeout(function () {
                //todo
                start();
            }, RECONNECT_INTERVAL);
        }
    });

    var getFunctionForEvent = function (eventName) {
        return function () {
            var handlers = eventHandlers[eventName].handlers;

            for (var i = 0; i < handlers.length; i++) {
                handlers[i].handler.apply(this, arguments);
            }
        }
    }

    //subscribe for all events
    for (var eventName in eventHandlers) {
        if (eventHandlers.hasOwnProperty(eventName)) {
            hub.on(eventName, getFunctionForEvent(eventName));
        }
    }

    SignalR.Hub = {
        start: start,

        onPingBack: function() {
            return eventHandlers["pingBack"];
        },

        onMessageReceived: function() {
            return eventHandlers["onMessageReceived"];
        },

        onSiteStopped: function () {
            return eventHandlers["onSiteStopped"];
        },

        
        onMessageDelivered: function() {
            return eventHandlers["messageDelivered"];
        },

        onMessageSent: function() {
            return eventHandlers["messageSent"];
        },

        stopSite: function () {
            queueOrCall(function () {
                /// <summary>Update user state</summary>
                /// <param name="user" type="Object"></param>
                hub.invoke('StopSite');
            });
        },

        sendMessage: function(message) {
            queueOrCall(function() {
                /// <summary>Update user state</summary>
                /// <param name="user" type="Object"></param>
                hub.invoke('SendMessage', message);
            });
        },

        onUserStateChanged: function(func) {
            /// <summary>Registers handler for userStateChanged event</summary>
            /// <param name="func" type="Function">Handler for event</param>
            return eventHandlers["userStateChanged"].add(func);
        },

        onConnected: function(handler) {
            connectedHandlers.push(handler);

            queueOrCall(handler);
        },

        updateUserState: function(user) {
            queueOrCall(function() {
                /// <summary>Update user state</summary>
                /// <param name="user" type="Object"></param>
                hub.server.updateUserState(user);
            });
        }
    };

})(SignalR = window.SignalR || {});
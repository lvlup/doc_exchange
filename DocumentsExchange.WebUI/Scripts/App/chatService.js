(function($, Chat, SignalR) {
    'use strict';

    var Creator = function (options) {
        this.init(options);
    };

    $.extend(Creator.prototype, {
        _renderMessage: function(message) {
            var tmpl = $.templates(this.options.templates.messageTemplate);
            var html = tmpl.render(message);
            $(this.options.controls.messageContainer).append(html);
        },

        _prepareMessage: function(message) {
            Object.defineProperty(message, 'time', {
                enumerable: true,
                get: function () {
                    var date = new Date(this.timeStamp);
                    return date.getHours() + ':' + date.getMinutes();
                }
            });

            Object.defineProperty(message, 'html', {
                enumerable: true,
                get: function() {
                    return (this.content || '').replace('\n', '<br/>');
                }
            });

            return message;
        },

        _addMessage: function (rawMessage, insertFirst) {
            var prepared = this._prepareMessage(rawMessage);
            if (insertFirst)
                this.messages.unshift(prepared);
            else {
                this.messages.push(prepared);
            }
            
            this._renderMessage(prepared);
        },

        _fetch: function(options) {
            return $.get(options.url, options.data).then(function (data) {
                while (data.items.length) {
                    this._addMessage(data.items.pop(), true);
                }

                return this.messages.length < data.total;
            }.bind(this)).fail(function(e) {
                console.log(e);
            });
        },

        _loadMessages: function(options) {
            this._fetch({
                url: options.urls.getMessagesUrl,
                data: $.param({
                    page: this._page++,
                    pageSize: this._pageSize
                })
            }).then(function (more) {
                $(options.controls.loadMoreBtn).toggle(more);
            }.bind(this));
        },

        _scrollDown: function () {
            var $container = $(this.options.controls.messageContainer);
            
            if ($container[0].scrollHeight - $container.scrollTop() - $container.outerHeight() > ($container.outerHeight() / 10))
                return;

            $container.scrollTop($container[0].scrollHeight);
        },

        init: function(options) {
            this.messages = [];
            this.options = options;
            this._page = 1;
            this._pageSize = 50;

            var sendMessage = function() {
                var $message = $(this.options.controls.messagePlaceholder);
                if (!$message.val())
                    return;

                var message = {
                    userId: this.options.data.userId,
                    userName: this.options.data.userName,
                    organizationId: 1,
                    content: $message.val(),
                    timeStamp: new Date().getTime()
                };

                $message.val('');

                this._addMessage(message);

                SignalR.Hub.sendMessage(message);

                this._scrollDown();
            }.bind(this);

            $(this.options.controls.messagePlaceholder).val('').focus();

            $(this.options.controls.loadMoreBtn)
                .click(function(e) {
                    this._loadMessages(this.options);
                }.bind(this)).hide();

            $(this.options.controls.messagePlaceholder).keypress(function(e) {
                if (e.keyCode === 13) {
                    sendMessage();
                    return false;
                }
            }.bind(this));

            $(options.controls.sendBtn).click(function (e) {
                sendMessage();
            }.bind(this));

            SignalR.Hub.onMessageReceived().add(function(message) {
                this._addMessage(message);
                this._scrollDown();
            }.bind(this));

            this._loadMessages(options);
        },

        start: function() {
            SignalR.Hub.start();
        }
    });

    Chat.instance = Creator;
})($, Chat = {}, SignalR)
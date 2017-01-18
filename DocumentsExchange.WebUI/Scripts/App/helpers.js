(function($, Helpers) {
    Helpers.submitForm = function (options) {
        var $promise = $.Deferred(),
            ajaxOptions = $.extend({
                type: "POST"
            }, options.ajaxOptions);

        $.ajax(ajaxOptions).done(function (data) {
            $promise.resolve(data);
            //$(options.controls.dataPlaceholder).html(data);
        }).fail(function (data) {
            var parsed = data.responseText
                ? JSON.parse(data.responseText)
                : { errors: [{ key: "Error", errors: "Something went wrong" }] };

            var unknown = [];
            parsed.errors.forEach(function (error) {
                var $element = options.form.find('[name$=' + error.key + '] + [data-val-field]');
                if (!$element.length)
                    unknown.push(error);
                else {
                    $element.html(error.errors);
                }
            });

            options.form.find('[data-val-summary]').html(unknown.reduce(function (x, y) { return x + y.key + ': ' + y.errors + '<br/>' }, ""));
            $promise.reject(data);
        });

        return $promise;
    };

    Helpers.tableResize = function(options) {
        var $heading = options.heading,// $('#usersTableHead'),
            $table = options.table,// $('#usersTable'),
            $bodyCells = $table.find('tbody tr:first').children(),
            colWidth;

        var resize = function () {
            // Get the tbody columns width array
            colWidth = $bodyCells.map(function () {
                return $(this).width();
            }).get();

            // Set the width of thead columns
            $heading.find('thead tr').children().each(function (i, v) {
                if (i === colWidth.length - 1)
                    return;

                $(v).width(colWidth[i]);
            });
        };

        // Adjust the width of thead cells when window resizes
        $(window).resize(resize).resize();
    };

    var ContentLoader = function(options) {
        this.init(options);
    };

    $.extend(ContentLoader.prototype, {
        Constructor: ContentLoader,
        init: function (options) {
            this.xhr = null;
            this.contentPlaceholder = options.content;
        },

        load: function (options) {
            if (this._url === options.url)
                return;

            this._url = options.url;

            if (this.xhr && this.xhr.readyState != 4) {
                this.xhr.abort();
            }
            
            this.contentPlaceholder.empty().addClass('progress');
            this.xhr = $.get(options.url).done(function (html) {
                this.contentPlaceholder.removeClass('progress').html(html);
            }.bind(this));
        }
    });

    Helpers.ContentLoader = ContentLoader;
})(jQuery, Helpers = {});
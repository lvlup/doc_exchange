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
})(jQuery, Helpers = {});
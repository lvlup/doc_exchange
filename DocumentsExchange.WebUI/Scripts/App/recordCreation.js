(function ($, RecordCreator) {
    'use strict';
    
    var Creator = function(options) {
        this.init(options);
    };

    $.extend(Creator.prototype, {
        init: function (options) {
            if (options.currency) {
                $(options.currency.currencyDropdown).change(function () {
                    var selectedId = $(this).val();

                    $.get($(this).data("url"), { currency: selectedId })
                        .done(function (data) {
                            $(options.currency.coursePlaceholder).val(data);
                        }).fail(function (jqXhr, textStatus) {
                            alert("Something went wrong: " + textStatus + ".");
                        });
                }).change();
            }

            $(options.controls.uploadBtn).change(function (e) {
                var files = this.files;
                if (files.length > 0) {
                    var fileArray = [];
                    for (var x = 0; x < files.length; x++) {
                        fileArray.push(files[x]);
                    }

                    $.post(options.urls.fileValidationUrl, { fileNames: fileArray.map(function (f) { return f.name }) })
                        .done(function (data) {
                            var invalidFiles = data.validationResult.filter(function (f) { return f.valid === false; });
                            if (invalidFiles.length > 0) {
                                alert("These files are invalid: " + invalidFiles.reduce(function(x, y) { return x + y.fileName + ' ' }, ""));
                                $(this).val('');
                            } else {
                                $(options.controls.fileNamePlaceholder).val(files[0].name);
                            }
                        }.bind(this));
                }
            });

            $(options.controls.addRecordBtn).click(function (e) {
                var $form = $(this).closest("form"),
                    dataArray = $form.serializeArray(),
                    act = options.urls.uploadUrl;

                $form.find('[data-val-field]').html('');

                e.preventDefault();

                var files = $(options.controls.uploadBtn)[0].files;
                var data = new FormData();
                if (files.length > 0) {
                    for (var x = 0; x < files.length; x++) {
                        data.append("file" + x, files[x]);
                    }
                } else {
                    if (!$(options.controls.fileNamePlaceholder).val()){}
                    {
                        $form.find('[name$=OriginalFileName] + [data-val-field]').html('Файл не выбран');
                        return;
                    }
                }

                dataArray.forEach(function (item) {
                    data.append(item.name, item.value);
                });

                $.ajax({
                    type: "POST",
                    url: act,
                    contentType: false,
                    processData: false,
                    data: data
                }).done(function (data) {
                    $(options.controls.dataPlaceholder).html(data);
                }).fail(function (data) {
                    var parsed = data.responseText
                        ? JSON.parse(data.responseText)
                        : { errors: [{ key: "Error", errors: "Something went wrong" }] };

                    var unknown = [];
                    parsed.errors.forEach(function (error) {
                        var $element = $form.find('[name$=' + error.key + '] + [data-val-field]');
                        if (!$element.length)
                            unknown.push(error);
                        else {
                            $element.html(error.errors);
                        }
                    });

                    $form.find('[data-val-summary]').html(unknown.reduce(function(x, y) { return x + y.key + ': ' + y.errors + '<br/>' }, ""));
                });
            });
        }
    });

    RecordCreator.manager = Creator;
})(jQuery, (RecordCreator = {}));
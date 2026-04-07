var App = function () {
    return {
        getView: function (sourcename, type, url, data, $htmldiv, success, error, aftersucces) {
            type = type || "GET";
            data = data || "{null}";
            if (url === null || url === undefined || url === "") { swalFail("Error", "Url cannot be Empty"); }
            if (type.toUpperCase() == "GET") {
                if (url.indexOf("?") > 0)
                    url = url + $.param(data);
                else
                    url = url + "?" + $.param(data);
                data = {};
            }
            var ajaxcall =
                $.ajax({
                    type: type,
                    url: url,
                    data: JSON.stringify(data),
                    contentType: "application/json; charset=utf-8",
                    element: $htmldiv,
                    success: function (successresult) {
                        if ($htmldiv !== null) {
                            $htmldiv.empty();
                            $htmldiv.html(successresult);
                        }
                        if (success !== null && success !== undefined)
                            success(successresult);

                    }, error: function (errorresult) {
                        if (error !== null && error !== undefined)
                            error(data);
                        else
                            swalFail('Error: ' + sourcename, errorresult.split('|')[1]);
                    }
                }); return ajaxcall;
        },
        getModel: function (sourcename, type, url, data, success, error, contentType) {
            if (contentType == undefined) contentType = "application/json; charset=utf-8",

                type = type || "GET"; data = data || "{null}";
            if (url === null || url === undefined || url === "") { swalFail("Error", "Url cannot be Empty"); }

            var ajaxcall =
                $.ajax({
                    type: type,
                    url: url,
                    data: JSON.stringify(data),
                    contentType: contentType,
                    success: function (successresult) {
                        if (success !== null && success !== undefined)
                            success(successresult);
                    }, error: function (errorresult) {
                        if (error !== null && error !== undefined)
                            error(data);
                        else
                            swalFail('Error: ' + sourcename, errorresult.split('|')[1]);
                    }
                }); return ajaxcall;
        },
        extractLast: function (term) {
            return split(term, /,\s*/).pop();
        },

        autoComplete: function ($inputField) {
            url = $inputField.attr('data-url');
            masterName = $inputField.attr('data-masterName');
            $inputField.autocomplete({
                source: function (request, response) {
                    if (!($inputField.is(':disabled'))) {
                        $.ajax({
                            url: url,
                            type: "POST",
                            dataType: "json",
                            data: { Prefix: request.term, MasterName: masterName },
                            success: function (data) {
                                response($.map(data, function (item) {
                                    //return item.Value;
                                    return { label: item.Value, value: item.Value, id: item.Id }
                                }))
                            },
                            error: function (response) {
                                alert(response.responseText);
                            },
                            failure: function (response) {
                                alert(response.responseText);
                            }
                        })
                    }
                },
                minLength: 3
            });
        },

        multiSelectAutoComplete: function ($inputField) {
            url = $inputField.attr('data-url');
            masterName = $inputField.attr('data-masterName');
            $inputField.autocomplete({
                source: function (request, response) {
                    tVal = request.term.replace(/\n/g, "; ");
                    if (split(tVal, /;\s*/).length > 1) {
                        $inputField.val(tVal.replace(/\;/g, ',') + ',');
                    }
                    if (extractLast(tVal).length >= 3) {
                        $.ajax({
                            url: url,
                            type: "POST",
                            dataType: "json",
                            data: { Prefix: extractLast(tVal), MasterName: masterName },
                            success: function (data) {
                                var selectedVal = $inputField.val().replace(/ /g, '').split(/,\s*/);
                                response($.map(data, function (item) {
                                    //return item.Value;
                                    if ($.inArray(item.Value, selectedVal) == -1) {
                                        return { label: item.Value, value: item.Value }
                                    }
                                    return { label: item.Value, value: item.Value, id: item.Id }
                                }))
                            },
                            error: function (response) {
                                alert(response.responseText);
                            },
                            failure: function (response) {
                                alert(response.responseText);
                            }
                        })
                    }
                },
                select: function (event, ui) {
                    var terms = split(this.value, /,\s*/);
                    // remove the current input
                    terms.pop();
                    // add the selected item
                    terms.push(ui.item.value);
                    // add placeholder to get the comma-and-space at the end
                    terms.push("");
                    this.value = terms.join(", ");
                    return false;
                },
            });
        },
        /////////////////
    };
}();
/// <reference path="jquery-1.6.2.js" />
/// <reference path="jquery.validate.js" />

$(function () {
    // Cache for dialogs
    var dialogs = {};

    var getValidationSummaryErrors = function ($form) {
        // We verify if we created it beforehand
        var errorSummary = $form.data('validation-summary-errors');
        if (!errorSummary) {
            errorSummary = $('<div class="validation-summary-errors"><span>Please correct the errors and try again.</span><ul></ul></div>')
                .insertBefore($form);

            // Remember that we created it
            $form.data('validation-summary-errors', errorSummary);
        }

        return errorSummary;
    };


    var formSubmitHandler = function (e) {
        var $form = $(this);

        // We check if jQuery.validator exists on the form
        if (!$form.valid || $form.valid()) {
            $.post($form.attr('action'), $form.serializeArray())
                .done(function (json) {
                    json = json || {};

                    // In case of success, we redirect to the provided URL or the same page.
                    if (json.success) {
                        location = json.redirect || location.href;
                    } else if (json.errors) {
                        var errorSummary = getValidationSummaryErrors($form);

                        var items = $.map(json.errors, function (error) {
                            return '<li>' + error + '</li>';
                        }).join('');

                        var ul = errorSummary
                            .find('ul')
                            .empty()
                            .append(items);
                    }
                });
        }

        // Prevent the normal behavior since we opened the dialog
        e.preventDefault();
    };

    var loadAndShowDialog = function (id, link, url) {
        var separator = url.indexOf('?') >= 0 ? '&' : '?';

        // Save an empty jQuery in our cache for now.
        dialogs[id] = $();

        // Load the dialog with the content=1 QueryString in order to get a PartialView
        $.get(url + separator + 'content=1')
            .done(function (content) {
                dialogs[id] = $('<div class="modal-popup">' + content + '</div>')
                    .hide() // Hide the dialog for now so we prevent flicker
                    .appendTo(document.body)
                    .filter('div') // Filter for the div tag only, script tags could surface
                    .dialog({ // Create the jQuery UI dialog
                        title: link.data('dialog-title'),
                        modal: true,
                        resizable: true,
                        draggable: true,
                        width: link.data('dialog-width') || 300
                    })
                    .find('form') // Attach logic on forms
                        .submit(formSubmitHandler)
                    .end();
            });
    };

    // List of link ids to have an ajax dialog
    var links = ['logonLink', 'registerLink'];

    $.each(links, function (i, id) {
        $('#' + id).click(function (e) {
            var link = $(this),
                url = link.attr('href');

            if (!dialogs[id]) {
                loadAndShowDialog(id, link, url);
            } else {
                dialogs[id].dialog('open');
            }

            // Prevent the normal behavior since we use a dialog
            e.preventDefault();
        });
    });

    function isAmountInValid(amtVal) {
        if (isNaN(amtVal))
            return true;
        if (parseFloat(amtVal) <= 0)
            return true;
        return false;
    }



    $("form#searchbox").submit(
        function (e) {
            var $form = $(this);

            // We check if jQuery.validator exists on the form
            if (!$form.valid || $form.valid()) {
                var amtVal = $("input#amount").val();
                if (amtVal.length == 0) {
                    $("h2#validationMessage").text("Please provide an amount.");
                    $("h2#validationMessage").show();
                    e.preventDefault();
                    return;
                }
                if (isAmountInValid(amtVal)) {
                    $("h2#validationMessage").text("Please enter a valid amount.");
                    $("h2#validationMessage").show();
                    e.preventDefault();
                    return;
                }
                $("h2#validationMessage").hide();
                $("#dialog-modal").dialog('open');
                var cl = new CanvasLoader('canvasloader-container'); //http://heartcode.robertpataki.com/canvasloader/
                cl.setColor('#1f1b1f'); // default is '#000000'
                cl.setShape('roundRect'); // default is 'oval'
                cl.setDiameter(189); // default is 40
                cl.setRange(1.1); // default is 1.3
                cl.setFPS(20); // default is 24
                cl.show(); // Hidden by default
                $.post($form.attr('action'), $form.serializeArray())
                .done(function (json) {
                    //json = json || {};

                    //                    // In case of success, we redirect to the provided URL or the same page.
                    //                    if (json.successsuccess) {
                    //                        location = json.redirect || location.href;
                    //                    } else if (json.errors) {
                    //                        var errorSummary = getValidationSummaryErrors($form);

                    //                        var items = $.map(json.errors, function (error) {
                    //                            return '<li>' + error + '</li>';
                    //                        }).join('');

                    //                        var ul = errorSummary
                    //                            .find('ul')
                    //                            .empty()
                    //                            .append(items);
                    //                    }
                    //                    var wholeTable = "";
                    //                    $.each(jsObj.quotesData, function (index, value) {
                    //                        var rowHtml = "";
                    //                        $.each(value, function (innerIndex, innerVal) {
                    //                            rowHtml += "<td>" + innerVal + "</td>";
                    //                        });
                    //                        wholeTable += "<tr>" + rowHtml + "</tr>";
                    //                    });
                    if (json.length > 0) {
                        $("div#tablePlaceHolder").html("");
                        $("div#tablePlaceHolder").append(json);
                        $("table.tablesorter").tablesorter();
                        $("table.tablesorter tbody tr:nth-child(1) td").css("color", "RED");

                    }
                    $("#dialog-modal").dialog('close');
                    cl.hide();
                });
            }
            e.preventDefault();
        });

    $("#dialog-modal").dialog({
        modal: true,
        autoOpen: false,
        resizable: false,
        position: ['center', 220],
        closeOnEscape: false,
        open: function (event, ui) {
            $("div.ui-dialog-titlebar").hide();
            $("div.ui-widget-content").css("background", "none");
            $("div.ui-widget-content").css("border", "none");
        }

    });
    $("#providers").dialog({
        modal: true,
        autoOpen: false,
        resizable: true,
        closeOnEscape: true,
        position: [700, 'center'],
        open: function (event, ui) {
            $("div.ui-dialog-titlebar").show();
            $("div.ui-widget-content").css("width", "500px");
            $("div.ui-widget-content").css("height", "300px");
            $("div.ui-widget-content").css("text-align", "center");
            $("div.ui-widget-content").css("background", "");
            
        }
    });
    $("a#linkProviders").click(function (e) {
        $("#providers").dialog('open');
        e.preventDefault();
    });

    // $("table.tablesorter").tablesorter();

    $(document).ready(function () {
        if (!Modernizr.input.placeholder) {
            var placeholderText = $('#amount').attr('placeholder');

            $('#amount').attr('value', placeholderText);
            $('#amount').addClass('placeholder');

            $('#amount').focus(function () {
                if (($('#amount').val() == placeholderText)) {
                    $('#amount').attr('value', '');
                    $('#amount').removeClass('placeholder');
                }
            });

            $('#amount').blur(function () {
                if (($('#amount').val() == placeholderText) || (($('#amount').val() == ''))) {
                    $('#amount').addClass('placeholder');
                    $('#amount').attr('value', placeholderText);
                }
            });
        }
    });

    $('body')
      .bind(
       'click',
       function (e) {
           if ($('#providers').dialog('isOpen')
         && !$(e.target).is('.ui-dialog, a')
         && !$(e.target).closest('.ui-dialog').length
        ) {
               jQuery('#providers').dialog('close');
           }
       }
      );


});
var _error_message_template = '<span class="field-validation-error[{CLASS}]"><span>[{ERROR}]</span></span>';

$(function () {

    $('form.validate').bind('submit', function (event) {

        event.preventDefault();

        var form = $(this);

        // Remove all the error messages
        form.find('.field-validation-error').remove();

        $.ajax({
            url: form.attr('action'),
            type: 'POST',
            dataType: 'json',
            data: form.serialize() + '&IsAjaxRequest=true',
            error: function (request, status, error) { },
            success: function (data, status, request) {
                if (data.length == 0) // If nothing is returned by the validator
                {
                    // We have to unbind first otherwise we have an infinite loop of validation...
                    form.unbind('submit');
                    form.submit();
                }
                else {
                    // Show validation messages
                    $.each(data, function (i, item) {
                        var element = $('#' + item.Property.replace(/\./gi, '_'));
                        element.closest('p').append(_error_message_template.replace(/\[\{ERROR\}\]/gi, item.Errors.join(', '))
                                                                           .replace(/\[\{CLASS\}\]/gi, (item.Property.indexOf('Other') != -1) ? ' other' : ''));
                    });
                }
            }
        });

    });

});
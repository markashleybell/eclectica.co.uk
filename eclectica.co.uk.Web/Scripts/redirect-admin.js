$(function () {
    $('#Redirect_RedirectID').bind('change', function () {
        var id = $(this).val();

        if (id != '')
            window.location = '/Redirect/Edit/' + id;
    });

    $('#AddButton').bind('click', function () {
        window.location = '/Redirect/Create';
    });

    $('#DeleteButton').bind('click', function () {
        if (confirm('Are you SURE you want to delete this redirect?'))
            window.location = '/Redirect/Delete/' + $('#Redirect_RedirectID').val();
    });

    $('form:first').bind('submit', function (event) {

        var f = $(this);

        return true;

    });
});
$(function () {
    $('#Link_LinkID').bind('change', function () {
        var id = $(this).val();

        if (id != '')
            window.location = '/Link/Edit/' + id;
    });

    $('#AddButton').bind('click', function () {
        window.location = '/Link/Create';
    });

    $('#DeleteButton').bind('click', function () {
        if (confirm('Are you SURE you want to delete this link?'))
            window.location = '/Link/Delete/' + $('#Link_LinkID').val();
    });

    $('form:first').bind('submit', function (event) {

        var f = $(this);

        return true;

    });
});
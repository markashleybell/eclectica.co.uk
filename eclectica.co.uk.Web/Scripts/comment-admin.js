$(function () {
    $('#Comment_CommentID').bind('change', function () {
        var id = $(this).val();

        if (id != '')
            window.location = '/Comment/Edit/' + id;
    });

    $('#AddButton').bind('click', function () {
        window.location = '/Comment/Create';
    });

    $('#DeleteButton').bind('click', function () {
        if (confirm('Are you SURE you want to delete this comment?'))
            window.location = '/Comment/Delete/' + $('#Comment_CommentID').val();
    });

    $('form:first').bind('submit', function (event) {

        var f = $(this);

        return true;

    });
});
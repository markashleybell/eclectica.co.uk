$(function () {
    $('#Entry_EntryID').bind('change', function () {
        var id = $(this).val();

        if (id != '')
            window.location = '/Entry/Edit/' + id;
        else
            window.location = '/Entry/Create';
    });

    $('#AddButton').bind('click', function () {
        window.location = '/Entry/Create';
    });

    $('#DeleteButton').bind('click', function () {
        if (confirm('Are you SURE you want to delete this post?'))
            window.location = '/Entry/Delete/' + $('#Entry_EntryID').val();
    });

    $('#imagepickerthumbs li').live('click', function () {
        insertImage($(this).attr('id').substring(1));
    });

    $('form:first').bind('submit', function (event) {

        var f = $(this);

        if (f.find('#Entry_Title').val() == '' && f.find('#Entry_Url').val() == '') {
            alert('You must manually fill in a URL if there is no post title');
            return false;
        }

        var suggested = [];

        $('#relatedposts li').each(function (i) {
            suggested.push($(this).attr('id').substring(7));
        });

        $('#related').val(suggested.join('|'));

        return true;

    });
});

function insertImage(id) {
    $('#Entry_Body').replaceSelection('<img alt="CAPTION" src="/img/lib/original/' + id + '.jpg" />', true);
}

function insertClass(cls) {
    $('#Entry_Body').replaceSelection('class="' + cls + '" ', true);
}

function insertCitation(cls) {
    $('#Entry_Body').replaceSelection('<p><cite>Via <a href=""></a></cite></p>', true);
}

function insertEntity(e) {
    $('#Entry_Body').replaceSelection(e, true);
}

function insertAnchor(e) {
    $('#Entry_Body').replaceSelection('<a href=""></a>', true);
}
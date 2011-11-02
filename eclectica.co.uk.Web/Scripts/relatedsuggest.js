

function AddRelatedSuggestion(item) {
    var info = item.split('^');

    $('#relatedposts').append('<li id="related' + info[0] + '">' + ((info[1] == '') ? info[2] : info[1]) + ' <a href="#" onclick="$(this).parent().remove(); return false;">x</a></li>');
    $('#relatedsuggestions').empty();
    $('#RelatedSearch').val('').focus();
}

function GetRelatedSuggestions(evt, o) {
    evt = (evt) ? evt : ((window.event) ? event : null);
    if (evt) {
        // If the key wasn't Esc, up or down arrow, and the tag search radio button is selected
        if (evt.keyCode != 27 && evt.keyCode != 38 && evt.keyCode != 40) {
            var letters = (o.value.indexOf(' ')) ? o.value.substring(o.value.lastIndexOf(' ') + 1).toLowerCase() : o.value.toLowerCase();

            if (letters == '') {
                $(document).unbind('keypress', handleEvents);
                $(document).unbind('keydown', handleEvents);
                $(document).unbind('keyup', handleArrowKeys);

                $('#relatedsuggestions').empty();
            }
            else {
                if (letters.length > 2) {
                    var item = new Array();
                    var items = new Array();

                    $.get('/Entry/RelatedSearch', { query: letters }, function (data) {

                        // console.log('data: ' + data);

                        items = data.split('|');

                        $('#relatedsuggestions').empty();

                        if (items.length <= 20) {

                            var tab = 1;
                            var s = new Array();

                            for (var x = (items.length - 1); x >= 0; ) {
                                item = items[x].split('^');
                                $('#relatedsuggestions').append('<li><a href="#" taborder="' + (tab++) + '" onclick="AddRelatedSuggestion(\'' + items[x].replace(/\'/gi, "\\'") + '\'); return false;">' + ((item[1] == '') ? item[2] : item[1]) + '</a></li>');
                                x--;
                            }
                        }
                        else {
                            $('#relatedsuggestions').append('<li>Too Many</li>');
                        }
                        // console.log(suggestionlist);

                        $(document).bind('keypress', handleEvents);
                        $(document).bind('keydown', handleEvents);
                        $(document).bind('keyup', handleArrowKeys);
                        // $('#suggestions').show();

                    });
                }
            }
        }
    }
}

$(function() {
    var q = $('#RelatedSearch').attr('taborder', '0');

    q.after('<ul id=\"relatedsuggestions\">&nbsp;</ul>');

    q.bind('keyup', function(event, o) {
        GetRelatedSuggestions(event, this);
    });
});
function handleEvents(evt) {
    evt = (evt) ? evt : ((window.event) ? event : null);
    if (evt) {
        switch (evt.keyCode) {
            case 38:
                return false;
            case 40:
                return false;
        }
    }
}

function handleArrowKeys(evt) {
    evt = (evt) ? evt : ((window.event) ? event : null);
    if (evt) {
        switch (evt.keyCode) {
            case 27:
                $(document).unbind('keypress', handleEvents);
                $(document).unbind('keydown', handleEvents);
                $(document).unbind('keyup', handleArrowKeys);
                $('#suggestions').html('&nbsp;');
                return false;
            case 38:
                if (suggestionindex > 0) {
                    suggestionindex--;
                    //console.log('Up: ' + suggestionindex + ': ' + $(suggestionlist[suggestionindex]).text());
                    $(suggestionlist[suggestionindex]).focus();
                }
                return false;
            case 40:
                if (suggestionindex < (suggestionlist.length - 1)) {
                    suggestionindex++;
                    //console.log('Down: ' + suggestionindex + ': ' + $(suggestionlist[suggestionindex]).text());
                    $(suggestionlist[suggestionindex]).focus();
                }
                return false;
        }
    }
}

function AddSuggestion(word) {
    var val = $('#Tags').attr('value');
    $('#Tags').attr('value', val.substring(0, val.lastIndexOf(' ') + 1) + word + ' ');
    $('#suggestions').html('&nbsp;');
    $('#Tags').focus();
}

var suggestionlist = null;
var suggestionindex = -1;

function GetSuggestions(evt, o) {
    evt = (evt) ? evt : ((window.event) ? event : null);
    if (evt) {
        // If the key wasn't Esc, up or down arrow, and the tag search radio button is selected
        if (evt.keyCode != 27 && evt.keyCode != 38 && evt.keyCode != 40) {
            var letters = (o.value.indexOf(' ')) ? o.value.substring(o.value.lastIndexOf(' ') + 1).toLowerCase() : o.value.toLowerCase();

            if (letters == '') {
                $(document).unbind('keypress', handleEvents);
                $(document).unbind('keydown', handleEvents);
                $(document).unbind('keyup', handleArrowKeys);

                $('#suggestions').html('&nbsp;');
            }
            else {
                if (letters.length > 2) {
                    var phrases = new Array();
                    var a = new Array();

                    $.get('/cms/ajax_get_suggestions.aspx', { q: letters }, function(data) {

                        // console.log('data: ' + data);

                        phrases = data.split('|');

                        for (var x = (phrases.length - 1); x >= 0; ) {
                            var word = phrases[x--].toLowerCase();
                            if (word.indexOf(letters) == 0) a[a.length] = word;
                        }

                        $('#suggestions').html('&nbsp;');

                        var tab = 1;
                        var s = new Array();

                        for (var x = (a.length - 1); x >= 0; )
                            $('#suggestions').append('<a href="#" taborder="' + (tab++) + '" onclick="AddSuggestion(\'' + a[x] + '\'); return false;">' + a[x--] + '</a> &nbsp; ');

                        suggestionlist = $('#suggestions a');
                        suggestionindex = -1;

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
    var q = $('#Tags').attr('taborder', '0');

    q.after('<div id=\"suggestions\">&nbsp;</div>');

    q.bind('keyup', function(event, o) {
        GetSuggestions(event, this);
    });
});
function activeUrls(text) {

    text = text.replace(/\s(http:\/\/[^\s\<]+)(\s)?/gi, ' <a href="$1">$1</a>$2');
    return text.replace(/(^|\s:?)@(\w+)/gi, ' <a href="http://twitter.com/$2">@$2</a>');
}

$(document).ready(function() {

    /*
    $.getJSON('/ajax_twitter.aspx', function(data) {

        var output = new Array();

        $.each(data, function(i, item) {

            output.push('<p>' + item.date + ': ' + activeUrls(item.text) + '</p>');

        });

        $('#tweets').html(output.join(''));

    });*/

    $('div.post img.captionb, div.post img.captionw, div.post img.captionn').each(function(i, item) {
        var img = $(item);
        var cls = img.attr('class');
        var caption = img.attr('alt');

        img.after('<div class="' + cls + '"><p>' + caption + '</p></div>');
    });
    
});
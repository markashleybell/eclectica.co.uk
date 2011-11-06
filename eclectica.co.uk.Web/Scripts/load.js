function activeUrls(text) {

    text = text.replace(/\s(http:\/\/[^\s\<]+)(\s)?/gi, ' <a href="$1">$1</a>$2');
    return text.replace(/(^|\s:?)@(\w+)/gi, ' <a href="http://twitter.com/$2">@$2</a>');
}

$(document).ready(function() {

    $.ajax({
        url: '/entry/recenttwitterstatuses',
        dataType: 'json',
        type: 'post',
        data: {
            count: 10
        },
        success: function (data, status, request) {
            
            var output = new Array();

            $.each(data, function(i, item) {

                output.push('<p>' + item.date + ': ' + activeUrls(item.status) + '</p>');

            });

            $('#tweets').html(output.join(''));

        },
        error: function (request, status, error) { 
        
            $('#tweets').html('<p>Error loading tweets.</p>');

        }
    });

    $('div.post img.captionb, div.post img.captionw, div.post img.captionn').each(function(i, item) {
        var img = $(item);
        var cls = img.attr('class');
        var caption = img.attr('alt');

        img.after('<div class="' + cls + '"><p>' + caption + '</p></div>');
    });

    window.___gcfg = { lang: 'en-GB' };

    (function () {
        var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;
        po.src = 'https://apis.google.com/js/plusone.js';
        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);
    })();
    
});
function GetResults(o)
{
	var letters = $.trim(o.value);

	if (letters.length < 2 || letters == '' || letters == 'the' || letters == 'and') 
	{
	    $('#searchresults').remove();
	    $('#contentbuffer').show();
	}
	else 
	{
	    $.post('/ajax_search.aspx', { q: letters }, RenderResults)
	}
}

function RenderResults(data)
{
    var results = new Array();

    var template = '<div class="post">' +
			            '<div class="postcontent">' +
			                '{%POSTDETAIL%}' +
			            '</div>' +
			            '<div class="postcontrols">' +
				            '<p>{%AUTHOR%}</p>' +
				            '<p class="cl">{%COMMENTS%}</p>' +
			            '</div>' +
			            '<div class="pclr">&nbsp;</div>' +
		            '</div>';
    
    if(data != '0')
	{
        if($('#searchresults').length == 0)	
	        $('#content').prepend('<div id="searchresults"></div>');
	    
		if(data.indexOf('~') == -1)
		{
		    results.push('<div class="post"><h3>' + data + ' results</h3><p>That\'s too many: type some more letters to narrow your search, or <a href="/tags/">click here to view popular tags.</a></p></div>');
		}
		else
		{
			var a = data.split('|').reverse();
			var r = '';
			
			for(var x=(a.length - 1); x>=0;)
			{
				r = a[x--].split('~');
				results.push(template.replace(/{%POSTDETAIL%}/gi, ((r[6] != '') ? '<img class="link-img" src="/img/lib/crop/' + r[6] + '" alt="" />' : '') + '<h3><a href="/' + r[0] + '/">' + ((r[2] == '') ? r[4] : r[2]) + '</a></h3><p>' + r[3] + '</p>').replace(/{%COMMENTS%}/gi, r[5] + ' comment' + ((r[5] != 1) ? 's' : '')).replace(/{%AUTHOR%}/gi, 'By ' + r[1]));
			}
		}

		$('#contentbuffer').hide();
		$('#searchresults').html('<p class="postdate">Search Results</p>' + results.join(''));
	}
	else
	{
	    $('#searchresults').remove();
	    $('#contentbuffer').show();
	}
}

$(document).ready(function () {

    $('#content').wrapInner('<div id="contentbuffer"></div>')

    //$('#search').bind('submit', function () { return false; });

    $('#search #query').attr('autocomplete', 'off')
    			       .bind('focus', function () { if (this.value == this.defaultValue) this.value = ''; })
	                   .bind('blur', function () { if (this.value == '') this.value = this.defaultValue; });
    //.bind('keyup', function() { GetResults(this); });
});
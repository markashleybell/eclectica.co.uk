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
	    $.post('/search', { query: letters, ajax: true }, RenderResults)
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
	    
		//if(data.indexOf('~') == -1)
		//{
		//    results.push('<div class="post"><h3>' + data + ' results</h3><p>That\'s too many: type some more letters to narrow your search, or <a href="/tags/">click here to view popular tags.</a></p></div>');
		//}
		//else
		//{
	    data.SearchResults.reverse();

	     

			for(var x=(data.SearchResults.length - 1); x>=0; x--)
			{
			    var jsonDate = new Date(+data.SearchResults[x].Published.replace(/\/Date\((\d+)\)\//, '$1'));
			    results.push(template.replace(/{%POSTDETAIL%}/gi, ((data.SearchResults[x].Thumbnail != '') ? '<img class="link-img" src="/content/img/lib/crop/' + data.SearchResults[x].Thumbnail + '" alt="" />' : '') + '<h3><a href="/' + data.SearchResults[x].Url + '/">' + ((data.SearchResults[x].Title == "") ? jsonDate : data.SearchResults[x].Title) + '</a></h3><p>' + data.SearchResults[x].Body + '</p>').replace(/{%COMMENTS%}/gi, data.SearchResults[x].CommentCount + ' comment' + ((data.SearchResults[x].CommentCount != 1) ? 's' : '')).replace(/{%AUTHOR%}/gi, 'By ' + data.SearchResults[x].Author.Name));
			}
		//}

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
	                   .bind('blur', function () { if (this.value == '') this.value = this.defaultValue; })
                       .bind('keyup', function() { GetResults(this); });
});
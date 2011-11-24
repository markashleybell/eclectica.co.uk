// Global variable to hold search result template HTML
// This means we only have to retrieve the template file on the first
// search per page load, instead of for every search
var _SEARCH_RESULT_HTML = '';

// Custom hacky date formatter - I was going to use Date.js, but it weighs in at 25kb which
// is a bit big considering it will only ever be used to render dates for title-less results...
function formatDate(d) {

    var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    var days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    var hours = d.getHours();
    var minutes = d.getMinutes();

    return days[d.getDay()] + ' ' +
            d.getDate() + ' ' +
            months[d.getMonth()] + ' ' +
            d.getFullYear() + ' ' +
            ((hours < 10) ? '0' + hours : hours) + ':' +
            ((minutes < 10) ? '0' + minutes : minutes);

}

// Tidy the query and POST it to our search action, 
// with the ajax flag set so it returns some nice JSON
function getResults(queryField) {

    var letters = $.trim(queryField.value);

    // Don't start searching until at least two characters have been typed
    if (letters.length < 2 || letters == '') {

	    $('#searchresults').remove();
	    $('#contentbuffer').show();

	}
	else {

	    $.post('/search', { query: letters, ajax: true }, getTemplate);

	}

}

// Retrieve the template HTML
function getTemplate(data) {

    // If the global var is empty
    if (_SEARCH_RESULT_HTML == '') {

        // Retrieve the template file and put the HTML content into the global var,
        // then pass both data and template to the rendering function
        $.get('/scripts/searchresult.tmpl', function (template) {

            _SEARCH_RESULT_HTML = template;
            renderResults(data, template);

        });

    }
    else {

        // Pass the 'cached' template HTML from the global to the rendering function
        renderResults(data, _SEARCH_RESULT_HTML);

    }

}

function renderResults(data, template) {

    var results = new Array();

    if (data.length) {

        if ($('#searchresults').length == 0)
            $('#content').prepend('<div id="searchresults"></div>');

        data.reverse();

        for (var x = (data.length - 1); x >= 0; x--) {

            // Deal with ASP.NET MVC's weird JSON date format
            var jsonDate = new Date(+data[x].Published.replace(/\/Date\((\d+)\)\//, '$1'));

            // Create a data model for this result
            var model = {
                url: data[x].Url,
                title: ((data[x].Title == "") ? formatDate(jsonDate) : data[x].Title),
                body: data[x].Body,
                author: data[x].Author.Name,
                comments: data[x].CommentCount + ' comment' + ((data[x].CommentCount != 1) ? 's' : ''),
                thumbnail: data[x].Thumbnail
            };

            // Render the HTML and push it to the result array
            results.push(Mustache.to_html(template, model));

        }

        $('#contentbuffer').hide();
        $('#searchresults').html('<p class="postdate">Search Results</p>' + results.join(''));

    }
    else {

        $('#searchresults').remove();
        $('#contentbuffer').show();

    }

}

$(document).ready(function () {

    $('#content').wrapInner('<div id="contentbuffer"></div>')

    // Stop the search form submitting to the 
    // standard (non-AJAX) search action if Enter is pressed
    $('#search').bind('submit', function () { return false; });

    $('#search #query').attr('autocomplete', 'off')
    			       .bind('focus', function () { if (this.value == this.defaultValue) this.value = ''; })
	                   .bind('blur', function () { if (this.value == '') this.value = this.defaultValue; })
                       .bind('keyup', function() { getResults(this); });
});
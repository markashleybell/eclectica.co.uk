$(document).ready(function() {

    if(!window.$CaptionInfo)
    {
    	$('body').append('<div id="captioninfo"></div>');
    	
    	window.$CaptionInfo = {
								obj: null,
								offsetX: 0,
								offsetY: 0
							};

		$CaptionInfo.obj = $('#captioninfo');
		$CaptionInfo.obj.hide();
	}
	
	var img = $('#monththumbnails a, #relatedposts a');
		
		 img.bind('mouseover', function(){
		        	$CaptionInfo.obj.html('<p>' + $(this).html() + '</p>');
		        	$CaptionInfo.offsetX = -20;//($CaptionInfo.obj.width() + 10);
					$CaptionInfo.offsetY = ($CaptionInfo.obj.height() + 20);
		        	$CaptionInfo.obj.show();
		        });
		        
		        img.bind('mousemove', function(e){
		        	$CaptionInfo.obj.css({ 'top': (e.pageY - $CaptionInfo.offsetY) + 'px', 'left': (e.pageX - $CaptionInfo.offsetX) + 'px',});
		        });
		        
		        img.bind('mouseout', function(){
		        	$CaptionInfo.obj.hide();
		        });
		
    

});
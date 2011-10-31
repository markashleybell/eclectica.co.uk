var fileList;     // Variable for holding the filelist.
var fileCount = 0;
var uploader;
var uploadButton;
var uploadOverlay;
var simultaneousUploads = 2;

$(document).ready(function () {

    uploadButton = $('#UploadButton');


    // Custom URL for the uploader swf file (same folder).
    YAHOO.widget.Uploader.SWFURL = "/content/img/site/uploader.swf";

    // Instantiate the uploader and write it to its placeholder div.
    uploader = new YAHOO.widget.Uploader("UploadButton", "/content/img/site/selectFileButton.png");

    // Add event listeners to various events on the uploader.
    // Methods on the uploader should only be called once the 
    // contentReady event has fired.

    uploader.addListener('contentReady', handleContentReady);
    uploader.addListener('fileSelect', onFileSelect)
    uploader.addListener('uploadStart', onUploadStart);
    uploader.addListener('uploadProgress', onUploadProgress);
    uploader.addListener('uploadCancel', onUploadCancel);
    uploader.addListener('uploadComplete', onUploadComplete);
    uploader.addListener('uploadCompleteData', onUploadResponse);
    uploader.addListener('uploadError', onUploadError);
    uploader.addListener('rollOver', handleRollOver);
    uploader.addListener('rollOut', handleRollOut);
    uploader.addListener('click', handleClick);

});


// When the mouse rolls over the uploader, this function
// is called in response to the rollOver event.
// It changes the appearance of the UI element below the Flash overlay.
function handleRollOver() {

}

// On rollOut event, this function is called, which changes the appearance of the
// UI element below the Flash layer back to its original state.
function handleRollOut() {

}

// When the Flash layer is clicked, the "Browse" dialog is invoked.
// The click event handler allows you to do something else if you need to.
function handleClick() {
    // Show depressed state of button!
}

// When contentReady event is fired, you can call methods on the uploader.
function handleContentReady() {
    // Allows the uploader to send log messages to trace, as well as to YAHOO.log
    // uploader.setAllowLogging(true);

    // Allows multiple file selection in "Browse" dialog.
    uploader.setAllowMultipleFiles(true);

    // New set of file filters (separate filter masks with semicolons).
    var ff = new Array({ description: "JPEG Images", extensions: "*.jpg" });

    // Apply new set of file filters to the uploader.
    uploader.setFileFilters(ff);
}

// Actually uploads the files. In this case,
// uploadAll() is used for automated queueing and upload 
// of all files on the list.
// You can manage the queue on your own and use "upload" instead,
// if you need to modify the properties of the request for each
// individual file.
function upload() {
    fileCount = 0;
    for (var i in fileList) fileCount++;

    if (fileList != null) {
        //uploader.setSimUploadLimit(((fileCount > simultaneousUploads) ? simultaneousUploads : fileCount));
        uploader.uploadAll("/Entry/UploadImage", "POST", {  }, "upload");
    }
}

// Fired when the user selects files in the "Browse" dialog
// and clicks "Ok".
function onFileSelect(event) {

    fileList = event.fileList;

    $('#SelectedFiles').empty();


    for (var i in fileList) {
        $('#SelectedFiles').append(fileList[i].name + ' <span id="' + fileList[i].id + '"></span><br />');
    }
}

// Do something on each file's upload start.
function onUploadStart(event) {

}

// Do something on each file's upload progress event.
function onUploadProgress(event) {
    var prog = Math.round(100 * (event["bytesLoaded"] / event["bytesTotal"]));
    $('#' + event["id"]).html(prog + '%');
}

// Do something when each file's upload is complete.
function onUploadComplete(event) {
    $('#' + event["id"]).html('Complete');
    uploader.removeFile(event["id"]);
    fileCount--;
}

// Do something if a file upload throws an error.
// (When uploadAll() is used, the Uploader will
// attempt to continue uploading.
function onUploadError(event) {
    $('#' + event["id"]).html('Error Uploading: ' + event.status);
}

// Do something if an upload is cancelled.
function onUploadCancel(event) {
    $('#' + event["id"]).html('Cancelled');
}

// Do something when data is received back from the server.
function onUploadResponse(event) {
    var images = event.data.split('|');
    var imageContainer = $('#imagepickerthumbs');

    var image = images[0].split(':');

    imageContainer.append('<li id="i' + image[1] + '"><span class="i" style="background-image: url(/img/lib/cms/' + image[1] + '.jpg);">' + image[1] + '</span><span>' + image[0] + '</span></li>');
}
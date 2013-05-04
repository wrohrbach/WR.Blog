$(document).ready(function () {
    if ($('textarea#Text').length > 0) {
        CKEDITOR.replace('Text');
    }

    if ($('textarea#Comment').length > 0) {
        CKEDITOR.replace('Comment', {
            toolbar: 'Basic',
            width: 465
        });
    }
});
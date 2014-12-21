$(document).ready(function () {
    $('[rel=tooltip]').tooltip();
    $('[rel=popover]').popover();

    $(document).on('click', '.close', function () {
        $(this).closest('.popover').prev('[rel=popover]').popover('hide');
    });
}); 
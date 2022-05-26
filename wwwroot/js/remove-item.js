$('#select').click(function () {
    $(this).addClass('d-none');
    $('#cancel').removeClass('d-none');
    $('#remove').removeClass('d-none');
    $('input[name="id"]').removeClass('d-none');
});
$('#cancel').click(function () {
    $('#select').removeClass('d-none');
    $(this).addClass('d-none');
    $('#remove').addClass('d-none');
    $('input[name="id"]').addClass('d-none');
    $('input[name="id"]').prop('checked', false);
    $('#remove').prop('disabled', true);
});
$('input[name="id"]').click(function () {
    $('#remove').prop('disabled', !$('input[name="id"]:checked').length);
});

$('form').submit(function () {
	$(this).find(':input')
		.filter(function () {
			return !$(this).val() || $(this).val() == '1' && ($(this).attr('name') == 'sort' || $(this).attr('name') == 'page');
		})
		.prop('name', '');
	return true;
});
$('form').find(':input:not([type=submit])').prop('disabled', false);

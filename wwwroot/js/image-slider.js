var imageSlider = {
	indexInternal: 0,
	get index() {
		return this.indexInternal;
	},
	set index(value) {
		this.indexInternal = value;
		turnImage();
	}
};

for (let i = 0; i < $('#image-panel img').length; ++i)
	$('#indicator-panel').append($('<button>', { class: 'indicator' })
		.click(e => imageSlider.index = $('.indicator').index(e.target)));
updateImageSizes();
imageSlider.index = 0;
$(window).resize(updateImageSizes);

$('#to-prev').click(() => --imageSlider.index);
$('#to-next').click(() => ++imageSlider.index);
$('#fullscreen-enter').click(e => {
	if ($(e.target).attr('id') == 'fullscreen-enter') {
		$(e.target).attr('id', 'fullscreen-exit');
		$('#image-slider').addClass('image-slider-fullscreen');
		$('html').addClass('overflow-hidden');
	}
	else {
		$(e.target).attr('id', 'fullscreen-enter');
		$('#image-slider').removeClass('image-slider-fullscreen');
		$('html').removeClass('overflow-hidden');
	}
	updateImageSizes();
});

function updateImageSizes() {
	$('#image-slider img').width($('#image-slider').width() + 'px');
	$('#image-slider img').height($('#image-panel').height() + 'px');
	$('#image-panel').css('margin-left', -imageSlider.index * $('#image-slider').width() + 'px');
}
function turnImage() {
	$('.indicator').prop('disabled', false);
	$('.indicator').eq(imageSlider.index).prop('disabled', true);
	if (!imageSlider.playInterval) {
		let imageCount = $('#image-panel img').length;
		$('#to-prev').prop('disabled', imageSlider.index == 0);
		$('#to-next').prop('disabled', imageSlider.index == imageCount - 1);
	}
	$('#image-panel').animate({ marginLeft: -imageSlider.index * $('#image-slider').width() + 'px'}, 400);
}

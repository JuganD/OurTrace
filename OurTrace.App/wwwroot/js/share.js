$(window).on("load", function() {
	$('#PostId').val($('input[name="postId"]').val())
	$('select').on("change",function() {
		if ($(this).val() === '2' || $(this).val() === '3') {
			$('#field-target').html($(this).children('option[value="'+$(this).val()+'"]').html().replace('wall',"name (case sensitive)"));
			$('#manual-field').fadeIn();
			$('#share-visibility').fadeOut();
		} else {
			$('#manual-field').fadeOut();
			$('#share-visibility').fadeIn();
		}
	});
});
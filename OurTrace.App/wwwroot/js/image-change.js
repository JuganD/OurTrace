$(window).on("load", function() {
	$(".image-change-menu").on("click", function() {
		$(".wrapper").addClass("overlay");
		$(".image-change-popup").addClass('active');
		let coverImage = $(".cover-sec").children("img").attr('src');
		let frontImage = $(".user-pro-img").children("a").children("img").attr('src');
		
		$("#current-cover-image").attr('src',coverImage);
		$("#current-front-image").attr('src',frontImage);
	});
	$(".close-change-image").on("click", function() {
		$(".image-change-popup").removeClass('active');
	});
	
});
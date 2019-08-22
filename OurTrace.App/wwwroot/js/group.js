$(window).on("load", function() {
	$("#leave-group-button").on("click",function () {
		$("#leave-group-box").fadeIn();
	});
	$("#leave-group-decline").on("click",function () {
		$("#leave-group-box").fadeOut();
	});
});
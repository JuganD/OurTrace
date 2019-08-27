$(window).on("load", function() {
	let currentLocation = $("#search-location").val();
	let categories = $("#categories-list").children("ul").children("li").children("a");

	$(categories).filter(function() {
		return $(this).html() == currentLocation;
	}).parent().addClass('active');
});
$(window).on("load", function() {
	let currentUserElevation = $("#current-user-elevation").val();
	let originalKickHref = $("#user-data-kick").attr("href");
	let promoteSubmitButtonValue = $("#user-data-promote-submit").val();
	$('[data-toggle="tooltip"]').tooltip();
	
	$(".member-link").click(function(){
		let profileLink = this.href;
		let img = $(this).children("img").first();
		let fullName = $(img).attr("search-title");
		let username = $(this).children("div").first().html();
		let usernameCleared = username.slice(1);
		let imgSrc = $(img).attr("src");
		let elevation = $(this).children('input[type="hidden"]').val();
		if (elevation == "0"){
			elevation = "Member";
		}
		let elevationAsInt = $(this).children('input[type="hidden"]').data('value');

		$("#none-message").fadeOut(200, function() {
						$("#user-data").fadeIn();
					});
		$("#user-data-img").attr("src",imgSrc);
		$("#user-data-name").html(username);
		$("#user-data-fullname").html(fullName);
		$("#user-data-elevation").html(elevation);
		$("#user-data-viewprofile").attr("href",profileLink);
		$("#user-data-kick-username").val(usernameCleared);
		$("#user-data-promote-username").val(usernameCleared);
		
		if (parseInt(currentUserElevation) > parseInt(elevationAsInt)){
			$("#promote-member-box").show();
			let elevationString = "";
			if (elevationAsInt == "0"){
				elevationString = "Moderator"
			}
			if (elevationAsInt == "1"){
				elevationString = "Administrator"
			}
			if (elevationString != ""){
				$("#user-data-promote-submit").val(promoteSubmitButtonValue+" to "+elevationString);
			}
			
			
		} else {
			$("#promote-member-box").hide();
		}
		
		return false;
	});
	
	$("#search-bar").click(function(){
		$("#user-data").fadeOut(200, function() {
						$("#none-message").fadeIn();
					});
		
		return false;
	});
});

function filterSearch() {
	// Declare variables
	let input, filter, ul, li, a, i, txtValue;
	input = document.getElementById('myInput');
	filter = input.value.toUpperCase();
	ul = document.getElementById("searchPanel");
	li = ul.getElementsByTagName('li');
	
	// Loop through all list items, and hide those who don't match the search query
	for (i = 0; i < li.length; i++) {
		a = li[i].getElementsByTagName("a")[0];
		txtValue = (a.textContent || a.innerText) + a.firstElementChild.getAttribute("search-title");
		if (txtValue.toUpperCase().indexOf(filter) > -1) {
			li[i].style.display = "";
			} else {
			li[i].style.display = "none";
		}
	}
}

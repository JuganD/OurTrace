$(window).on("load", function() {
	updateScroll();
	$('#send-message-box').focus();
	let recipientName = $("#recipient-name").html();
	$("#recipient-input").val(recipientName);
	
	$('#send-message-box').keypress(function (e) {
		var key = e.which;
		if(key == 13)  // the enter key code
		{
			$(this).parent().children('button[type="submit"]').click();
			return false;  
		}
	}); 
});
	
function updateScroll(){
    var element = document.getElementById("chatbox-chat-container");
    element.scrollTop = element.scrollHeight;
}
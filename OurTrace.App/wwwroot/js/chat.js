$(window).on("load", function() {
	updateScroll();
	$('#Content').focus();
	let recipientName = $("#recipient-name").html();
	$("#recipient-input").val(recipientName);
	
	$(".message-send-area").on("click",function() {
		$('#Content').focus();
	});
});
	
function updateScroll(){
    var element = document.getElementById("chatbox-chat-container");
    element.scrollTop = element.scrollHeight;
}
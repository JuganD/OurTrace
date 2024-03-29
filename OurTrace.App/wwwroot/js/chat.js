$(window).on("load", function() {
	updateScroll();
	$('#Content').focus();
	var recipientName = $("#recipient-name").html();
	var currentUsername = $("#current-user-name").val();
	
	$(".message-send-area").on("click",function() {
		$('#Content').focus();
	});
	
	$(document).on('keypress',function(e) {
		if(e.which == 13) {
			$('#sendButton').trigger('click');
		}
	});
	
	var connection =
            new signalR.HubConnectionBuilder()
                .withUrl("/chathub")
				.withHubProtocol(new signalR.protocols.msgpack.MessagePackHubProtocol())
                .build();

        connection.on("NewMessage",
            function (message) {
                AddMessageToChat(message,true);
            });

        $("#sendButton").click(function() {
            let messageText = $("#message-input-box").val();
            connection.invoke("Send",recipientName,messageText);
			$("#message-input-box").val("");
			
			let currentMessage = {Sender:currentUsername, Content:messageText};
			AddMessageToChat(currentMessage,false);
        });

        connection.start().catch(function (err) {
            return console.error(err.toString());
        });
});

function AddMessageToChat(message,isSender) {
	let class1 = "st3";
	let class2 = "st3";
	if (isSender == false){
		class1 = "ta-right";
		class2 = "";
	}
	
	let messageContent = message.Content == undefined ? message.content : message.Content;
	let messageSender = message.Sender == undefined ? message.sender : message.Sender;
    var chatMessage = "<div class=\"main-message-box "+class1+"\">"+"<div class=\"message-dt "+class2+"\">"+"<div class=\"message-inner-dt\">"+"<p>"+messageContent+"</p>"+"</div>"+"<span style=\"display:inline-block\">"+GetDateStringFromDate(new Date())+"</span>"+"</div>"+"<div class=\"messg-usr-img\">"+"<img src=\"/File/ProfilePicture/"+messageSender+"\""+"alt=\"Avatar\" style=\"width:100%;\">"+"</div>"+"</div>";
    $("#chatbox-chat-container").append(chatMessage);
	updateScroll();
}
	
function updateScroll(){
    var element = document.getElementById("chatbox-chat-container");
    element.scrollTop = element.scrollHeight;
}
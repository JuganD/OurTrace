$(window).on("load", function() {
	AssertEmptyMessage();
	AssertOnClickForClass(".accept-req","lightgreen");
	AssertOnClickForClass(".close-req","#FF4747");
});
function AssertOnClickForClass(className,color){
	$(className).on("click",function () {
		let acceptButton = this;
		
        let form = $('#_AjaxAntiForgeryRequestAccept');
        let token = $('input[name="__RequestVerificationToken"]', form).val();
		let username = $(".accept-feat").prev().children().html();
		
        $.ajax({
            url: $(this).data('url'),
            type: 'POST',
            data: { 
                __RequestVerificationToken: token, 
                receiver: username
            },
            success: function () {
				$(acceptButton).closest('.request-details')
					.css("background-color",color)
					.fadeOut(1000, function() {
						$(this).remove();
						AssertEmptyMessage();
					});
			}	
        });
		
        return false;
    });
}
function AssertEmptyMessage() {
	if ($(".request-details").length == 0){
		let noFriendRequestsElement = $("<div style=\"display:none;\" class=\"text-center p-4\">No new friend requests!</div>");
		$(".requests-list").append(noFriendRequestsElement);
		$(noFriendRequestsElement).fadeIn();
	}
}
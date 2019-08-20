$(window).on("load", function() {
	$('[data-toggle="tooltip"]').tooltip();
	AssertEmptyMessage();
	$(".joinRequest-accept").on("click",function () {
		let joinRequestElement = this;
		
        let form = $('#_AjaxAntiForgeryRequestAccept');
        let token = $('input[name="__RequestVerificationToken"]', form).val();
		let username = $(this).prev().children().children().html();
		let groupname = $("#groupName").html();
		
        $.ajax({
            url: $(this).data('url'),
            type: 'POST',
            data: { 
                __RequestVerificationToken: token, 
                groupname: groupname,
				membername: username
            },
            success: function () {
				$(joinRequestElement).closest('.suggestion-usd')
					.css("background-color","lightgreen")
					.fadeOut(1000, function() {
						$(this).remove();
						AssertEmptyMessage();
					});
			}	
        });
		
        return false;
    });
});

function AssertEmptyMessage() {
	if ($(".suggestion-usd").length == 0){
		let noJoinRequestElement = $("<div class=\"text-center\" style=\"display: none;\">No join requests.</div>");
		$("#requests-list").append(noJoinRequestElement);
		$(noJoinRequestElement).fadeIn();
	}
}
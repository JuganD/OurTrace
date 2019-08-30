var originalFunc = CallHtmlPosts;
$(window).on("load", function() {
	
	$(window).scroll(function() {
		if ($("#posts-spinner").visible()){
			CallHtmlPosts();
		}
	});
});

function CallHtmlPosts(){
	CallHtmlPosts = Dull;
	let spinner = $("#posts-spinner");
	let form = $(spinner).parent();
    let token = $('input[name="__RequestVerificationToken"]', form).val();

	$.ajax({
            url: $(form).attr("action"),
            type: 'POST',
            data: { 
                __RequestVerificationToken: token
            },
            success: function (htmlResult) {
				var dom_nodes = $("<div>" + htmlResult + "</div>").children();
				$(dom_nodes).children('form').remove();
				$(dom_nodes).insertBefore($(spinner).parent());
				$(dom_nodes).children(".profiles-slider").each(function (){
					ProfileSliderSlick($(this));
				});
				DateToLocal($(dom_nodes).find(".utcDate"));
				//console.log($(dom_nodes).find(".utcDate"));
			},
			fail: function (jqXHR, textStatus, errorThrown) {
				console.log(textStatus); 
			}
        });
	
	setTimeout(function(){ 
		CallHtmlPosts = originalFunc;
	}, 5000);
}
function Dull() {};
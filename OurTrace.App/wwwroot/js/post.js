window.onload = function(){
	DateToLocal('span.utcDate');
	
	InsertReadMoreButton('.postContent');
	
	$(".com").click(function() {
		let commentSec = $(this).closest(".post-bar").next("div.comment-section");
		if (commentSec.is(":visible")){
			commentSec.hide();
		} else {
			commentSec.show();
		}
		return false;
	});
	
	$(".post-like").on("click", function() {
		let likeButton = this;
	
		let postId = $(this).closest(".post-bar").children('input[name="postId"]').val();
		let form = $('#_AjaxAntiForgeryPost');
        let token = $('input[name="__RequestVerificationToken"]', form).val();
		
		$.ajax({
            url: $(this).data('url'),
            type: 'POST',
            data: { 
                __RequestVerificationToken: token, 
                postId: postId
            },
            success: function () {
				let likesSelector = $(likeButton).parent().children('.likes-count').first();
				let newLikesHtml = $(likesSelector).html().replace(/\d+$/, function(n){ return ++n });
				$(likesSelector).html(newLikesHtml);
				$(likeButton).children('li').first().children('a').addClass('active');
				$(likeButton).off('click');
			},
			fail: function (jqXHR, textStatus, errorThrown) {
				console.log(textStatus); 
			}
        });
	});
	
	$(".post-comment-send").on("click", function() {
		let postButton = this;
	
		let username = $("#user-identity").html();
		let postId = $(this).closest(".comment-section").closest(".posty").children('.post-bar').children('input[name="postId"]').val();
		let content = $(this).closest('.comment_box').children('input').val();
		let form = $('#_AjaxAntiForgeryPost');
        let token = $('input[name="__RequestVerificationToken"]', form).val();
		let commentSection = $(this).closest('.comment-section').children('.comment-sec').children('ul');
		
		let date = new Date();
		let dateString = GetDateStringFromDate(date);
		
		$.ajax({
            url: $(this).data('url'),
            type: 'POST',
            data: { 
                __RequestVerificationToken: token, 
                postId: postId,
				content: content
            },
            success: function () {
				$(commentSection).append("<li><div class=\"comment-list\"><div class=\"bg-img\"><img src=\"/file/profilepicture/"+username+"\" style=\"width:40px; height:40px;\" alt=\"\"></div><div class=\"comment\"> <h3><a class=\"text-black-50\">"+username+"</a></h3><img src=\"/images/clock.png\"><span class=\"utcDate\">"+dateString+"</span><p class=\"postContent\">"+content+"</p></div></div></li>");
			},
			fail: function (jqXHR, textStatus, errorThrown) {
				console.log(textStatus); 
			}
        });
	});
	$('.post-comment-content').keypress(function (e) {
		var key = e.which;
		if(key == 13)  // the enter key code
		{
			$(this).parent().children(".post-comment-send").click();
			$(this).val('');
			return false;  
		}
	});   
}
function InsertReadMoreButton(selector){
	$(selector).each(function() {
		if (isOverflown(this)){
			($('<button class="more btn btn-sm">See more </button>').fadeIn(800).on('click', function() {
				$(this).prev().css({'max-height':'100%', 'height':'auto'});
				$(this).hide();
			})).insertAfter($(this));
		}
	});
}
function isOverflown(element) {
    return element.scrollHeight > element.clientHeight || element.scrollWidth > element.clientWidth;
}

function DateToLocal(selector){
	let currentDates = [];
	$(selector).each(function () {
		currentDates.push(this.innerHTML);
	});
	
	let arrayLength = currentDates.length;
	for (let i = 0; i < arrayLength; i++) {
		let dateStr = JSON.parse(currentDates[i]);
		if (dateStr.slice(-1) != "Z"){
			dateStr = dateStr+"Z";
		}
		currentDates[i] = new Date(dateStr);
	}
	
	let counter = 0;
	$(selector).each(function () {
		let d = currentDates[counter];
		var datestring = GetDateStringFromDate(d);
		this.innerHTML = datestring;
		$(this).fadeIn();
		counter++;
	});
}
function time_with_leading_zeros(time) 
{ 
  return (time < 10 ? '0' : '') + time;
}
function GetDateStringFromDate(date){
	return date.getDate()  + "/" + (date.getMonth()+1) + "/" + date.getFullYear() + " " + time_with_leading_zeros(date.getHours()) + ":" + time_with_leading_zeros(date.getMinutes()) + ":" + time_with_leading_zeros(date.getSeconds());
}

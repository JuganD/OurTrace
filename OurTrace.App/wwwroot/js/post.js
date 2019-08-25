window.onload = function(){
	$('[data-toggle="tooltip"]').tooltip();
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
				let likesSelector = $(likeButton).closest(".job-status-bar").children('.likes-count').first();
				let newLikesHtml = $(likesSelector).html().replace(/\d+$/, function(n){ return ++n });
				$(likesSelector).html(newLikesHtml);
				$(likeButton).addClass('active');
				$(likeButton).off('click');
			},
			fail: function (jqXHR, textStatus, errorThrown) {
				console.log(textStatus); 
			}
        });
	});
	
	$(".post-comment-like").on("click", function() {
		let likeButton = this;
	
		let postId = $(this).closest(".comment-section").closest(".posty").children(".post-bar").children('input[name="postId"]').val();
		let commentId = $(this).closest(".comment").children('input[name="commentId"]').val();
		let form = $('#_AjaxAntiForgeryPost');
        let token = $('input[name="__RequestVerificationToken"]', form).val();
		
		$.ajax({
            url: $(this).data('url'),
            type: 'POST',
            data: { 
                __RequestVerificationToken: token, 
                postId: postId,
				commentId: commentId
            },
            success: function () {
				let likesSelector = $(likeButton).closest(".comment").children('.post-comment-likes').first();
				let newLikesHtml = $(likesSelector).html().replace(/\d+$/, function(n){ return ++n });
				$(likesSelector).html(newLikesHtml);
				$(likeButton).addClass('active');
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
	$(".post-delete").on("click", function() {
		let deleteButton = this;
		
		let form = $('#_AjaxAntiForgeryPost');
        let token = $('input[name="__RequestVerificationToken"]', form).val();
		let post = $(this).closest('.posty');
		let middleElement = $("<div class=\"middle-element\" style=\"width:50%\"><span class=\"form-control mb-2 text-center\">Are you sure?</span><button class=\"form-control mb-1 btn-danger post-delete-yes\">Yes</button><button class=\"form-control btn-secondary post-delete-no\">No</button></div>");
		let postId = $(post).children('.post-bar').children('input[name="postId"]').val();
		
		
		$(post).append(middleElement);
		$(post).addClass("brightness-low");
		$(middleElement).center({ against: post });
		$(post).addClass("brightness-max");
		$(post).children(".post-bar").addClass("brightness-low");
		$(post).children(".post-bar").children(".post_topbar").children(".ed-opts").children(".ed-options").removeClass("active");
		
		$(".post-delete-no").on("click", function() {
			$(middleElement).remove();
			$(post).removeClass("brightness-low");
			$(post).removeClass("brightness-max");
			$(post).children(".post-bar").removeClass("brightness-low");
			
			$(this).unbind();
		});
		$(".post-delete-yes").on("click", function() {
			$.ajax({
				url: $(deleteButton).data('url'),
				type: 'POST',
				data: { 
					__RequestVerificationToken: token, 
					postId: postId
				},
				success: function () {
					$(post).fadeOut(800, function() {
						$(post).remove();
					});
				},
				fail: function (jqXHR, textStatus, errorThrown) {
					console.log(textStatus); 
				}
			});
			$(this).unbind();
		});
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

/*! Copyright 2011, Ben Lin (http://dreamerslab.com/)
* Licensed under the MIT License (LICENSE.txt).
*
* Version: 1.1.1
*
* Requires: jQuery 1.2.6+
*/
;(function($,window){var get_win_size=function(){if(window.innerWidth!=undefined)return[window.innerWidth,window.innerHeight];else{var B=document.body;var D=document.documentElement;return[Math.max(D.clientWidth,B.clientWidth),Math.max(D.clientHeight,B.clientHeight)]}};$.fn.center=function(opt){var $w=$(window);var scrollTop=$w.scrollTop();return this.each(function(){var $this=$(this);var configs=$.extend({against:"window",top:false,topPercentage:0.5,resize:true},opt);var centerize=function(){var against=configs.against;var against_w_n_h;var $against;if(against==="window")against_w_n_h=get_win_size();else if(against==="parent"){$against=$this.parent();against_w_n_h=[$against.width(),$against.height()];scrollTop=0}else{$against=$this.parents(against);against_w_n_h=[$against.width(),$against.height()];scrollTop=0}var x=(against_w_n_h[0]-$this.outerWidth())*0.5;var y=(against_w_n_h[1]-$this.outerHeight())*configs.topPercentage+scrollTop;if(configs.top)y=configs.top+scrollTop;$this.css({"left":x,"top":y})};centerize();if(configs.resize===true)$w.resize(centerize)})}})(jQuery,window);

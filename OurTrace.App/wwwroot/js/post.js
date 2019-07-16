window.onload = function(){
	DateToLocal('span.utcDate');
	
	InsertReadMoreButton('.postContent');
	
	$(".com").click(function() {
		console.log("work");
		let commentSec = $(this).closest(".post-bar").next("div.comment-section");
		if (commentSec.is(":visible")){
			commentSec.hide();
		} else {
			commentSec.show();
		}
		return false;
	});
}
function InsertReadMoreButton(selector){
	$(selector).each(function() {
		if (isOverflown(this)){
			($('<button class="more btn btn-sm">Read more </button>').on('click', function() {
				$(this).prev().css({'height':'auto'});
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
		currentDates[i] = new Date(dateStr);
		
	}
	
	let counter = 0;
	$(selector).each(function () {
		let d = currentDates[counter];
		var datestring = d.getDate()  + "/" + (d.getMonth()+1) + "/" + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds();
		this.innerHTML = datestring;
		counter++;
	});
}
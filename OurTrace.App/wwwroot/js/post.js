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
		var datestring = d.getDate()  + "/" + (d.getMonth()+1) + "/" + d.getFullYear() + " " + time_with_leading_zeros(d.getHours()) + ":" + time_with_leading_zeros(d.getMinutes()) + ":" + time_with_leading_zeros(d.getSeconds());
		this.innerHTML = datestring;
		$(this).fadeIn();
		counter++;
	});
}
function time_with_leading_zeros(time) 
{ 
  return (time < 10 ? '0' : '') + time;
}

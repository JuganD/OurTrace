window.onload = function(){
	let currentDates = [];
	$('span.utcDate').each(function () {
		currentDates.push(this.innerHTML);
	});
	
	let arrayLength = currentDates.length;
	for (let i = 0; i < arrayLength; i++) {
		let dateStr = JSON.parse(currentDates[i]);  
		currentDates[i] = new Date(dateStr);
		
	}
	
	let counter = 0;
	$('span.utcDate').each(function () {
		let d = currentDates[counter];
		var datestring = d.getDate()  + "/" + (d.getMonth()+1) + "/" + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds();
		this.innerHTML = datestring;
		counter++;
	});
	
	$('.postContent').each(function() {
		if (isOverflown(this)){
			($('<button class="more btn btn-sm">Read more </button>').on('click', function() {$(this).prev().css({'height':'auto'});})).insertAfter($(this));
		}
	});
}

function isOverflown(element) {
    return element.scrollHeight > element.clientHeight || element.scrollWidth > element.clientWidth;
}
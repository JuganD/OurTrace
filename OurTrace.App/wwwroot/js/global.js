$(window).on("load", function() {
	DateToLocal('span.utcDate');
	
	$("#notifications-clear").on("click", function() {
		let notificationButton = this;
		let form = $('#_AjaxAntiForgeryNotifications');
        let token = $('input[name="__RequestVerificationToken"]', form).val();
		
		$.ajax({
            url: $(notificationButton).data('url'),
            type: 'POST',
            data: { 
                __RequestVerificationToken: token,
				redir: false
            },
            success: function () {
				$(".notfication-details").addClass("bg-lightgray");
				$("#notification-count").removeClass("bg-danger");
				$("#notification-count").html("0");
			},
			fail: function (jqXHR, textStatus, errorThrown) {
				console.log(textStatus); 
			}
        });
	});
});

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
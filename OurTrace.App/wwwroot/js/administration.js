$(window).on("load", function() {
	$("#ModifyExistingButton").on("click", function() {
		$("#IdBox").fadeIn(200);
		$("#SubmitButton").val("Modify");
	});
	$("#CreateNewButton").on("click", function() {
		$("#Id").val("");
		$("#IdBox").fadeOut(200);
		$("#IssuerName").val("");
		$("#Content").val("");
		$("#Type").val($("#Type option:first").val());
		$("#ViewsLeft").val("");
		$("#SubmitButton").val("Create");
	});
	$(".clickable-row").on("click", function() {
		if ($("#Id").is(":visible")){
			let tableBlock = $(this).children("td");
			let tableId = GetTableChildrenValue(tableBlock,"Id");
			let tableIssuerName = GetTableChildrenValue(tableBlock,"IssuerName");
			let tableContent = GetTableChildrenValue(tableBlock,"Content");
			let tableType = GetTableChildrenValue(tableBlock,"Type");
			let tableViewsLeft = GetTableChildrenValue(tableBlock,"ViewsLeft");
		
			$("#Id").val(tableId);
			$("#IssuerName").val(tableIssuerName);
			$("#Content").val(tableContent);
			$('#Type option:contains("'+tableType+'")').prop('selected', true)
			$("#ViewsLeft").val(tableViewsLeft);
		}
		
	});
});
function GetTableChildrenValue(tableObj, name) {
	return $(tableObj).children('span[name="'+name+'"]').html();
}
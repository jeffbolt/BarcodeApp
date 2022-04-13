function uploadBarcode() {
	var fname = $("#FileUploadBarcode").val().toLowerCase();
	if (fname.substr(fname.length - 5) != ".jpeg" &&
		fname.substr(fname.length - 4) != ".jpg" &&
		fname.substr(fname.length - 4) != ".jpe" &&
		fname.substr(fname.length - 4) != ".gif" &&
		fname.substr(fname.length - 4) != ".png") {
		alert($("Error: Invalide file type.").val());
		return false;
	}

	$("#btnUploadBarcode").prop("disabled", true);
	var frm = document.forms[0];
	frm.submit();
	return true;
}
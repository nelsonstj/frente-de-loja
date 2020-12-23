function fileValidation() {
    var fileInput = document.getElementById("file");
    var filePath = fileInput.value;
    var allowedExtensions = /(\.csv)$/i;
    if (!allowedExtensions.exec(filePath)) {
        $("#modal-protheus").modal();
        //alert("Por favor, selecione um arquivo .csv válido.");
        fileInput.value = "";
        return false;
    }
    return true;
}
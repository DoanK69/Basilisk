var btnChecks = document.querySelectorAll("[name='check']");
btnChecks.forEach(function (checkbox) {
    checkbox.addEventListener("change", function () {
        var aChecked = checkbox.nextElementSibling;
        aChecked.click();
    });
});


var btnChecksAll = document.querySelectorAll("[name='checkAll']");
btnChecksAll.forEach(function (checkbox) {
    checkbox.addEventListener("change", function () {
        var aChecked = checkbox.nextElementSibling;
        aChecked.click();
    });
});

$('#myModal').on('shown.bs.modal', function () {
    $('#myInput').trigger('focus')
})


const selectDelivery = document.getElementById('delivery');

selectDelivery.addEventListener('change', function () {
    const selectedOption = selectDelivery.options[selectDelivery.selectedIndex];
    var checked = selectedOption.nextElementSibling;
    checked.click();
});

//var shippedDate = document.getElementById("shippedDate").value;
//var actionLink = document.getElementById("shipped");

//if (shippedDate != " ") {
//    actionLink.Attributes["disabled"] = "true";
//}
//else {
//    actionLink.Attributes["disabled"] = "false";
//}
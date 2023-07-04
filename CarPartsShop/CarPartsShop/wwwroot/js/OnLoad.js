
$(document).ready(function () {
    if ($("#pay-now").is(":checked")) {
        $("#card-info").removeAttr("hidden");
    }
    else if ($("#pay-leter").is(":checked")) {
        $("#card-info").attr("hidden", true);
    }
});

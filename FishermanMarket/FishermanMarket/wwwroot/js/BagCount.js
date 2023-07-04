
function GetCount() {
    $.ajax({
        url: `/Bags/Details?handler=Count`,
        success: function (data) {
            $(`#bag-count`).text(`${data}`);
        }
    })
}
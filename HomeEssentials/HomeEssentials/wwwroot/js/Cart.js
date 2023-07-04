let total = document.getElementById("total");

$(".count").on("change", function () {
    var count = $(this).val();
    var itemId = $(this).attr("item-id");

    $.ajax({
        url: `/Carts/Details?handler=ChangeCount&itemId=${itemId}&count=${count}`,
        success: function (data) {
            total.textContent = `${Number(data).toFixed(2)} лв.`
        }
    })
})
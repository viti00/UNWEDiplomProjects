let total = document.getElementById("total");

function Remove(id) {
    if (confirm("Наистина ли желаете да премахнете това ястие от торбичката?")) {
        $.ajax({
            url: `/Bags/Details?handler=Remove&id=${id}`,
            success: function (data) {
                total.textContent = `${Number(data).toFixed(2)} лв.`
                window.location.href = `/Bags/Details`;
            }
        })
    }
}

$(".quantity").on("change", function () {
    var newCount = $(this).val();
    var pId = $(this).attr("data-id");

    $.ajax({
        url: `/Bags/Details?handler=ChangeProductCount&pId=${pId}&newCount=${newCount}`,
        success: function (data) {
            total.textContent = `${Number(data).toFixed(2)} лв.`
        }
    })
})
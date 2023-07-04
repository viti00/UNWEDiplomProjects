$(".quantity").on("change", function () {
    var newCount = $(this).val();
    var pId = $(this).attr("pId")

    Action(pId, newCount);
})

$(".down").on("click", function () {
    let inputEl = this.parentNode.querySelector('input[type=number]');
    inputEl.stepDown();
    var newCount = Number(inputEl.getAttribute("value")) - 1;
    inputEl.setAttribute("value", newCount.toString())
    var pId = inputEl.getAttribute("pId");

    Action(pId, newCount)
})

$(".up").on("click", function () {
    let inputEl = this.parentNode.querySelector('input[type=number]');
    inputEl.stepUp();
    var newCount = Number(inputEl.getAttribute("value")) + 1;
    inputEl.setAttribute("value", newCount.toString())
    var pId = inputEl.getAttribute("pId");

    Action(pId, newCount)
})

function Action(pId, newCount) {
    $.ajax({
        url: `?handler=EditDetails&newCount=${newCount}&pId=${pId}`,
        success: function (data) {
            $.ajax({
                url: `?handler=Price&pId=${pId}`,
                success: function (data) {
                    $(`#${pId}`).text(`${data.toFixed(2)} лв.`);
                },
                error: function (error) {
                    alert("Error: " + error);
                }
            })
            $.ajax({
                url: `?handler=PriceTotal`,
                success: function (data) {
                    $(`#price-total`).text(`${data.toFixed(2)} лв.`);
                },
                error: function (error) {
                    alert("Error: " + error);
                }
            })
        },
        error: function (error) {
            alert("Error: " + error);
        }
    })
}
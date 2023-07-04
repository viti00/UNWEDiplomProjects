
$(".remove-btn").on("click", function () {

    if (confirm("Сигурни ли стe, че искате да махнете този продукт от вашата количка?")) {
        var partId = $(this).attr("part-id");

        var tr = $(`tr[part-id='${partId}']`).first();

        tr.remove();

        $.get(`/Carts/RemovePartFromCart?partId=${partId}`, function () {
            updateCartCount();
            $.get("/Carts/GetNewTotalPrice", function (totalPrice) {
                var strongElement = $('#total-price');
                strongElement.text(`${totalPrice} лв.`);
            })
        })
    }
});


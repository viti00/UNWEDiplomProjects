function GetCount() {
    $.ajax({
        url: `/ShoppingCarts/Info?handler=ProductsCount`,
        success: function (data) {
            $(`#cartCount`).text(`${data}`);
        }
    })
}
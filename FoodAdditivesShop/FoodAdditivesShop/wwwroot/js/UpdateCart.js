function UpdateCart() {
    $.ajax({
        url: `/Carts/Details?handler=CartCount`,
        success: function (data) {
            $(`#cartCount`).text(`${data}`);
        }
    })
}


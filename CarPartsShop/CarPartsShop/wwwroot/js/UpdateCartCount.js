function updateCartCount() {
    $.ajax({
        type: "GET",
        url: "/Carts/GetCartPartsCount",
        success: function (count) {
            var el = document.getElementById("cartCount");
            el.textContent = count;
        },
        error: function () {
            console.log("Възникна грешка при извличането на броя на продуктите в количката.");
        }
    });
}


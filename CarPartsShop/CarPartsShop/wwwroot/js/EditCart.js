
    $(".count-el").on("input", function () {
        var newCount = $(this).val();
        var partId = $(this).attr("part-id")

        $.ajax({
            type: 'POST',
            url: '/carts/EditPartCount',
            data: { newCount: newCount, partId: partId },
            success: function (response) {
                if (response) {
                    $.get("/Carts/GetNewTotalPrice", function (totalPrice) {
                        var strongElement = $('#total-price');
                        strongElement.text(`${totalPrice} лв.`);
                    })
                }
                else {
                    alert("За съжаление нямаме необходимото количество от този продукт за вашата поръчка!");
                    location.reload();
                }
            },
            error: function () {
                alert('Възникна проблем, моля опитайте пак!');
            }
        });
   })

let btn = document.getElementById("add-btn");
btn.addEventListener("click", Add);


function Add() {
    let count = document.getElementById("quantity").value;
    let partId = btn.getAttribute("part-id");
    $.ajax({
        type: 'POST',
        url: '/carts/AddToCart',
        data: { count: count, partId: partId },
        success: function (response) {
            debugger;
            if (response) {
                alert(`Успешно добавихте ${count} броя от този продукт!`)
                updateCartCount();
            }
            else {
                alert(`За съжаление нямаме достатъчно количество от този продукт за вашата поръчката!`)
            }
            
        },
        error: function () {
            alert('Възникна грешка, моля опитайте отново!');
        }
    });
}
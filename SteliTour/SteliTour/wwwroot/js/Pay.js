let id = "";

$("#pay").on("click", function () {
    id = $(this).attr("data-id");
    $('.modal').modal('show');
});

$('.close').click(function () {
    $('#reservationModal').modal('hide');
});

$("#send").on("click", function () {
    let amount = document.getElementById("amountInput").value;
    var remaining = document.querySelector(`#remaining[data-id="${id}"]`);

    if (amount < 1) {
        alert("Моля въведете валидна сума")
    }
    else if (Number(remaining.textContent) - Number(amount) < 0) {
        alert("Опитвате се да добавите повече пари, отколкото е необхоидмо")
    }
    else {
        window.location.href = `/Reservations/PaymentModel?id=${id}&amount=${amount}`;
    }
});
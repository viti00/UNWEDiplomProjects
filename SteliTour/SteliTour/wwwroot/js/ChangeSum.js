function Change(id) {
    let el = document.getElementById("new-sum");
    let sum = el.value;
    var payed = document.querySelector(`#payed[data-id="${id}"]`);
    var remaining = document.querySelector(`#remaining[data-id="${id}"]`);
    if (sum < 1) {
        alert("Моля въведете валидна сума")
    }
    else if (Number(remaining.textContent) - Number(sum) < 0) {
        alert("Опитвате се да добавите повече пари, отколкото е необхоидмо")
    }
    else {
        $.ajax({
            url: `/Reservations/Edit?handler=ChangeSum&id=${id}&sum=${sum}`,
            success: function (data) {
                payed.textContent = `${Number(payed.textContent) + Number(sum)}`
                remaining.textContent = `${data}`
                el.value = 0;
            }
        })
    }
   
}
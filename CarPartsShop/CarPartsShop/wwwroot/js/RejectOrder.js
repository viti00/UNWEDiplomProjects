function Reject(id) {
    if (confirm("Сигурни ли сте, че искате да откажете поръчката")) {
        $.ajax({
            type: "POST",
            url: "/Orders/RejectOrder",
            data: { id: id },
            success: function (result) {
                if (result) {
                    ChangeStatus(id)
                } else {
                    alert("Неуспешно отхвърляне на поръчката.");
                }
            },
            error: function () {
                alert("Възникна грешка при изпълнението на заявката.");
            }
        });
    }
    
}

function ChangeStatus(id) {
    var element = document.querySelector(`[data-id="${id}"] #status`);
    var reject = document.querySelector(`[data-id="${id}"] #reject`);
    reject.remove();
    element.textContent = "Отказана";
}
function Check(id) {
    console.log(id);
    debugger;
    $.ajax({
        url: `/Reservations/Create?handler=Check&id=${id}`,
        success: function (data) {
            if (data == true) {
                alert("Вече имате резервация за тази почивка!")
                window.location.href = `/Destinations/Index`;
            }
        }
    })
}
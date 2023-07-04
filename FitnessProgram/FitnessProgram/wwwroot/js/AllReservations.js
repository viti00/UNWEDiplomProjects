$('#datepicker').change(function () {
    var id = $(this).val().toString();
    $.get('GetAllReservations/' + id, function (response) {
        if (response.length == 0) {
            $('.no-res').removeClass("display-none")
            $(".timeslots").addClass("display-none");
            $(".res").addClass("display-none")
        }
        else {
            $(".res").removeClass("display-none")
            $('.no-res').addClass("display-none")
            for (var i = 0; i < response.length; i++) {
                let element = document.querySelector(`[range-id="${response[i].rangeId}"]`)
                element.children[1].textContent = response[i].name
                element.children[2].textContent = response[i].phone
                element.children[3].textContent = response[i].address
                element.children[4].textContent = response[i].email
                element.children[5].innerHTML = `<button class="btn btn-danger" onclick="DeleteReservation('${response[i].reservationId}')">Reject</button>`;
                $(element).removeClass("display-none");
            }
            $(".timeslots").removeClass("display-none");
        }
        
    });

});
$(".timeslot").on("click", function () {
    var rangeId = $(this).attr("range-id");
    var hour = $(this).find("span").text().split(":")[0];
    if (CheckTimeRange(hour) == false) {
        return;
    }
    var dateId = $('#datepicker').val().toString();

    $('#rangeIdInput').val(rangeId);
    $('#dateIdInput').val(dateId);
    $('.modal').modal('show');
});

$('.close').click(function () {
    $('#reservationModal').modal('hide');
});

$('#datepicker').change(function () {
    var id = $(this).val().toString();
    if (CheckData(id) == false) {
        $(".timeslots").addClass("display-none");
        return;
    }
    $.ajax({
        url: `/Customers/GetRanges/?id=${id}`,
        success: function (response) {
            var object = JSON.parse(response);
            var values = object["$values"];
            for (var i = 0; i < values.length; i++) {
                
                let element = document.querySelector(`[range-id="${i}"]`)
                if (values[i].Status == "Taken") {
                    $(element).prop("disabled", true).addClass("disabled");
                }
                else {
                    $(element).prop("disabled", false).removeClass("disabled");
                }
            }
            $(".timeslots").removeClass("display-none");
        }
    })
    

});

function CheckData(pickedDate) {
    let selectedDate = new Date(pickedDate);
    selectedDate.setHours(0, 0, 0, 0);

    let today = new Date();
    today.setHours(0, 0, 0, 0);

    if (selectedDate < today) {
        alert("The selected date is before today");
        return false;
    }

    return true;
}

function CheckTimeRange(hour) {
    var now = new Date();
    if (hour.length == 1) {
        hour = `0${hour}`
    }
    var pickedDate = $('#datepicker').val() + `T${hour}:00:00`
    var selectedDate = new Date(pickedDate);
    var tenHoursFromNow = new Date(now.getTime() + (10 * 60 * 60 * 1000));
    if (selectedDate < tenHoursFromNow) {
        alert("You cannot book a workout that starts in less than 10 hours");
        return false;
    }
    return true;
}


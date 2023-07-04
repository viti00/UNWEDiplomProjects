function Edit(id) {
    var el = document.querySelector(`[data-id="${id}"]`);

    var name = el.children[1].textContent;
    var address = el.children[2].textContent;
    var phone = el.children[3].textContent; 

    $('#nameInput').val(name);
    $('#addressInput').val(address);
    $('#phoneInput').val(phone);
    $('#reservationId').val(id);

    $("#name").addClass("display-none")
    $("#phone").addClass("display-none")
    $("#address").addClass("display-none")

    $('.modal').modal('show');
}

function SaveChanges(e) {
    e.preventDefault();

    const form = document.querySelector('#reservationForm');
    const inputs = form.querySelectorAll('input');

    var name = inputs[0].value;
    var phone = inputs[1].value
    var address = inputs[2].value;

    var id = inputs[3].value;

    $("#name").addClass("display-none")
    $("#phone").addClass("display-none")
    $("#address").addClass("display-none")

    if (name != "" && phone != "" && address != "") {
        $.ajax({
            type: "POST",
            url: "/Customers/EditReservation",
            data: { id: id, name: name, phone: phone, address: address },
            success: function (result) {
                if (result) {
                    alert("Changes saved successfully.");
                    location.reload();
                } else {
                    alert("Failed to save changes.");
                }
            },
            error: function () {
                alert("An error occurred while saving changes.");
            }
        });
    }
    else {
        if (name == "") {
            $("#name").removeClass("display-none")
        }
        if (phone == "") {
            $("#phone").removeClass("display-none")
        }
        if (address == "") {
            $("#address").removeClass("display-none")
        }
    }
}

$('.close').click(function () {
    $('#reservationModal').modal('hide');
});

function Reject(id) {
    swal({
        title: "Are you sure?",
        text: "Once canceled, you will not be able to use this reservation!",
        icon: "warning",
        buttons: ["Cancel", "Yes!"],
        dangerMode: true,
    })
    .then((willDelete) => {
        if (willDelete) {

            swal("The reservation was canceled!", {
                icon: "success",
            })
                .then(() => {
                    $.ajax({
                        type: "POST",
                        url: "/Customers/RejectReservation",
                        data: { "id": id },
                        success: function (result) {
                            DeleteRow(id)
                        },
                        error: function () {
                            alert("Failed to cancel reservation.");
                        }
                    });
                });


        } else {
            swal("The reservation will not be canceled!");
        }
    });
}

function DeleteRow(id) {
    var element = document.querySelector(`[data-id="${id}"]`);
    element.remove();
}
function DeleteReservation(id) {
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover this reservation!",
        icon: "warning",
        buttons: ["Cancel", "Yes, delete it!"],
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {

                swal("The reservation was deleted!", {
                    icon: "success",
                })
                    .then(() => {
                        $.get(`/admin/customers/delete/${id}`, () => {
                            window.location.replace("/Admin/Customers/AllReservations");
                        });
                    });


            } else {
                swal("The reservation will not be deleted!");
            }
        });
}
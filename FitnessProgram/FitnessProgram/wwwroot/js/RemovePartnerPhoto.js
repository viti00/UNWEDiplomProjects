function Remove(id, postId) {

    let table = document.getElementById("table");

    $.ajax({
        type: "POST",
        url: "/admin/Partners/RemovePhoto",
        data: { id: id, postId: postId },
        success: function (response) {
            if (response) {
                table.remove();
                Hide()

            }
            else {
                alert("Try again");
            }

        },
        error: function (xhr, textStatus, errorThrown) {
            alert("An error has occurred, please try again")
        }
    });
}
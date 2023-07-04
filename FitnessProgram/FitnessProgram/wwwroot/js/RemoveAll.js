
function RemoveAll(postId) {
    debugger;
    $.ajax({
        type: "POST",
        url: "/Posts/RemoveAll",
        data: { postId: postId },
        success: function (response) {
            if (response) {
                let table = document.getElementById("table");

                table.innerHTML = "";

                Hide();
                
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
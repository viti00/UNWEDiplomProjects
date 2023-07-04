function Remove(id, postId) {


    let element = document.querySelector(`[data-id="${id}"]`);
    let table = document.getElementById("table");

    $.ajax({
        type: "POST",
        url: "/admin/BestResults/RemovePhoto",
        data: { id: id, postId: postId },
        success: function (response) {
            if (response) {
                debugger;
                table.removeChild(element);

                if (table.children.length == 0) {
                    Hide()
                }
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

function RemoveAll(postId) {
    debugger;
    $.ajax({
        type: "POST",
        url: "/admin/BestResults/RemoveAll",
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
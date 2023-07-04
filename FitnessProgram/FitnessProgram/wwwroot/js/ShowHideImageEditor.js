$("#edit-image").on("click", Show)
let element = document.getElementsByClassName("img-editor")[0];
let textElement = document.getElementById("edit-image");

function Show() {

    textElement.classList.add("display-none");
    element.classList.remove("display-none");
}

function Hide() {
    textElement.classList.remove("display-none");
    element.classList.add("display-none");
}
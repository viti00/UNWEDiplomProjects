let prevControl = document.querySelector(".prev-btn");
let nextControl = document.querySelector(".next-btn");
let photosCount = document.querySelectorAll(".carousel img").length;
debugger;
let firstElement = document.querySelector(".active");
let firstElPos = Number(firstElement.getAttribute("position"));

if (firstElPos == 0) {
    prevControl.classList.add("display-none");
}

if (photosCount <= 1) {
    nextControl.classList.add("display-none");
}

function Left() {
    let activeElement = document.querySelector(".active");

    let previousPosition = Number(activeElement.getAttribute("position")) - 1;

    if (previousPosition < 0) {
        previousPosition = 0;
    }

    let previousElement = document.querySelector(`[position="${previousPosition}"]`);

    activeElement.classList.remove("active");
    activeElement.classList.add("display-none");

    previousElement.classList.remove("display-none");
    previousElement.classList.add("active");

    if (nextControl.classList.contains("display-none")) {
        nextControl.classList.remove("display-none");
    }

    if (previousPosition == 0) {

        prevControl.classList.add("display-none");
    }
}

function Right() {
    debugger;
    let activeElement = document.querySelector(".active");

    let nextPosition = Number(activeElement.getAttribute("position")) + 1;

    if (nextPosition > photosCount - 1) {
        nextPosition = photosCount - 1;
    }

    let nextElement = document.querySelector(`[position="${nextPosition}"]`);

    activeElement.classList.remove("active");
    activeElement.classList.add("display-none");

    nextElement.classList.remove("display-none")
    nextElement.classList.add("active");


    if (prevControl.classList.contains("display-none")) {
        prevControl.classList.remove("display-none");
    }

    if (nextPosition == photosCount - 1) {
        nextControl.classList.add("display-none");
    }
}
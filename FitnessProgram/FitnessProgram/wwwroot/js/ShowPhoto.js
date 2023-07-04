function ShowPhoto(i) {
    if (i === 'more') {
        i = 0;
    }

    if (i == 0) {
        prevControl.classList.add("display-none");
        if (document.querySelector(`[position="${i + 1}"]`) != undefined) {
            nextControl.classList.remove("display-none");
        }
    }
    if (i == photosCount - 1) {
        nextControl.classList.add("display-none");
        if (document.querySelector(`[position="${i - 1}"]`) != undefined) {
            prevControl.classList.remove("display-none");
        }
    }

    element.classList.add("d-flex");
    let activeElement = document.querySelector(`[position="${i}"]`);
    activeElement.classList.add("active");
}
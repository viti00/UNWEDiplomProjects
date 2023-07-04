const starCheckboxes = document.querySelectorAll('.star');
const otherElement = document.querySelector('#input-rating');

starCheckboxes.forEach(function (starCheckbox) {
    starCheckbox.addEventListener('change', function () {
        if (this.checked) {
            otherElement.value = this.value;
        }
    });
});


function FillStars(raiting) {
    starCheckboxes.forEach(function (checkbox) {
        if (Number(checkbox.value) == raiting) {
            checkbox.checked = true;
        }
    });

}



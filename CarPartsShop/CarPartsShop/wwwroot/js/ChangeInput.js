let monthEl = $('#month-el');
let yearEl = $('#year-el');

$(document).ready(function () {

    monthEl.on('input', function () {
        var text = $(this).val();
        if (text.length == 2) {
            yearEl.focus();
        }
    });
    yearEl.on('input', function () {
        var text = $(this).val();
        if (text.length == 0) {
            monthEl.focus();
        }
    });
});
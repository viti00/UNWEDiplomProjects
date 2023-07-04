
$(".carousel-control-prev").on("click", function () {
    var activeElement = document.querySelector('.active');

    if (activeElement) {
        var currentIndex = activeElement.getAttribute('data-index');

        var prevElement = document.querySelector('[data-index="' + (Number(currentIndex) - 1) + '"]');

        if (prevElement) {
            prevElement.classList.add('active');
            if ((Number(currentIndex) - 1) == 0) {
                $(".carousel-control-prev").attr("hidden", true);
            }
            activeElement.classList.remove('active'); 
            $(".carousel-control-next").removeAttr("hidden");
        }
               
    }
});

$(".carousel-control-next").on("click", function () {
    var activeElement = document.querySelector('.active');

    if (activeElement) {
        var currentIndex = activeElement.getAttribute('data-index');

        var nextElement = document.querySelector('[data-index="' + (Number(currentIndex) + 1) + '"]');

        var elements = document.querySelectorAll('[data-index]');
        var lastIndex = $(elements).last().attr("data-index");

        if (nextElement) {
            nextElement.classList.add('active');
            if ((Number(currentIndex) + 1) == Number(lastIndex)) {
                $(".carousel-control-next").attr("hidden", true);
            }
            activeElement.classList.remove('active');
            $(".carousel-control-prev").removeAttr("hidden");
        }
    }
});
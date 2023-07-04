
$("#plane").on("click", async function () {
    const plane = $('#plane');
    //await plane.animate({ rotate: '-=30deg' }).promise();
    for (let i = 0; i < 100; i++) {
        await new Promise(resolve => setTimeout(resolve, 10));
        plane.css({ 'transform': `translateX(${i - 2}px) translateY(${i / -10 -10}px)` });
    }

    var id = plane.attr('data-id');
    window.location.replace(`/Reservations/Create?id=${id}`);
});
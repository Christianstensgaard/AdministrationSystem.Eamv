$(document).ready(function () {
    startTime();
});

function startTime() {
    var today = new Date();
    var h = today.getHours();
    var m = today.getMinutes();
    var s = today.getSeconds();

    m = checkTime(m);
    s = checkTime(s);

    $('#time').html(h + ":" + m + ":" + s);

    var t = setTimeout(function () {
        startTime()
    }, 500); // updates every half a second
}

function checkTime(i) {
    if (i < 10) {
        i = "0" + i
    };
    return i;
}
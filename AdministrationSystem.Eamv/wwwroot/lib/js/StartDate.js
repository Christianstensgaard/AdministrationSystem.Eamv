$(document).ready(function () {
    startDate();
});

function startDate() {
    var today = new Date();

    var dd = today.getDate();
    var ddd = today.getDay();
    var day = new Array("mandag", "tirsdag", "onsdag", "torsdag", "fredag", "lørdag", "søndag");
    var mm = today.getMonth();
    var mth = new Array("januar", "febuar", "marts", "april", "maj", "juni", "juli", "august", "september", "october", "november", "december");
    var yy = today.getFullYear();

    $('#date').html(day[(ddd - 1 == -1 ? '6' : ddd - 1)] + ", " + dd + ". " + mth[mm] + " " + yy);

    var t = setTimeout(function () {
        startDate()
    }, 3600000); // updates every hour
}
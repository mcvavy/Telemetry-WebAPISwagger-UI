
$("form").submit(function (e) {

    var ch1 = $("#ch1").val();
    var ch2 = $("#ch2").val();
    if (ch1 === "" || ch2 === "") {

        e.preventDefault();
        return false;
    } else {
        return true;
    }

});
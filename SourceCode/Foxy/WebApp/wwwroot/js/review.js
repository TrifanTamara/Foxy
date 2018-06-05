jQuery(document).ready(function () {
    $('#see-answer').hide();

    $("#eye-button").click(function (e) {
        if ($('#see-answer').is(':visible')) {
            $('#see-answer').hide();
        } else {
            $('#see-answer').show();
        }
    });
});

jQuery(document).ready(function () {
    $('#see-answer').hide();

    $("#eye-button").click(function (e) {
        if ($('#see-answer').is(':visible')) {
            $('#see-answer').hide();
        } else {
            $('#see-answer').show();
        }
    });

    var textInput = document.getElementById('wanakana-input');
    wanakana.bind(textInput, /* options */); // uses IMEMode with toKana() as default
    // to remove event listeners: wanakana.unbind(textInput);

    (function () {
        'use strict';
        window.addEventListener('load', function () {
            // Fetch all the forms we want to apply custom Bootstrap validation styles to
            var forms = document.getElementsByClassName('needs-validation');
            // Loop over them and prevent submission
            var validation = Array.prototype.filter.call(forms, function (form) {
                form.addEventListener('submit', function (event) {
                    if (validateForm() === false) {
                        event.preventDefault();
                        event.stopPropagation();
                    }
                    form.classList.add('was-validated');
                }, false);
            });
        }, false);
    })();
});

function validateForm() {
    var result = true;
    var meaning = document.forms["review-form"]["meaning-input"].value;
    var reading = document.forms["review-form"]["reading-input"].value;

    //if (!wanakana.isKana(reading)) {
    //    result = false;
    //    $("#div-validate-reading").text("This field should contain only kana characters");
    //} else
    //if (reading == "") {
        result = false;
        $("#div-validate-reading").text("This field should not be empty");
//    }

    if (!wanakana.isKana(reading)) {
        result = false;
        $("#div-validate-reading").text("This field should contain only kana characters");
    } 

    if (meaning == "") {
        result = false;
        $("#div-validate-meaning").text("This field should not be empty");
    }
    return result;
}
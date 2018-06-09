jQuery(document).ready(function () {
    $('#see-answer').hide();
    ClearResponseBoxes();
    $("#eye-button").addClass("inactive-button");
    $('#eye-button').tooltip('disable');
    $('#nextButton').hide();

    $("#eye-button").click(function (e) {
        ManageEyeButton();
    });
    
    var textInput = document.getElementById('wanakanainput');
    wanakana.bind(textInput, /* options */);
});

function ManageEyeButton() {
    if ($("#eye-button").hasClass("inactive-button") == false) {
        $('#eye-button').tooltip('enable');
        if ($('#see-answer').is(':visible')) {
            $('#see-answer').hide();
        } else {
            HideInputs();
            $('#see-answer').show();
        }
    } else {
        $('#eye-button').tooltip('disable');
        toastr.warning("You need to submit your answer first before see the right answer.");
    }
}

function validateForm(form) {
    var reading = form.wanakanainput.value;
    var meaning = form.meaninginput.value;
    if ($('#submitButton').is(':visible')) {
        var result = true;

        if ($('#div-reading-wanakana').is(':visible')) {
            if (reading == "") {
                result = false;
                $("#wanakanainput").addClass("is-invalid");
                $("#wanakanainput").removeClass("is-valid");
                $("#div-validate-reading").text("This field should not be empty.");
            } else if (!wanakana.isKana(reading)) {
                result = false;
                $("#wanakanainput").addClass("is-invalid");
                $("#wanakanainput").removeClass("is-valid");
                $("#div-validate-reading").text("This field should contain only kana characters.");
            } else {
                $("#wanakanainput").addClass("is-valid");
                $("#wanakanainput").removeClass("is-invalid");
            }
        }

        if (meaning == "") {
            result = false;
            $("#meaninginput").addClass("is-invalid");
            $("#meaninginput").removeClass("is-valid");
            $("#div-validate-meaning").text("This field should not be empty.");
        } else {
            $("#meaninginput").addClass("is-valid");
            $("#meaninginput").removeClass("is-invalid");
        }

        if (result) {
            $("#eye-button").removeClass("inactive-button");
            $('#eye-button').tooltip('enable');
            $('#eye-button').tooltip('show');
            document.getElementById('meaninginput').readOnly = true;
            document.getElementById('wanakanainput').readOnly = true;
            $('#submitButton').hide();
            $('#nextButton').show();

            CheckAnswerServer(meaning, reading);
        }
    } else {
        $('#submitButton').show();
        $('#nextButton').hide();
        $("#eye-button").addClass("inactive-button");
        $('#eye-button').tooltip('disable');
        $('#eye-button').tooltip('hide');
        ClearResponseBoxes();

        form.wanakanainput.value = "";
        form.meaninginput.value = "";

        document.getElementById('meaninginput').readOnly = false;
        document.getElementById('wanakanainput').readOnly = false;

        GetNextReview();
    }
    return false;
}

function CheckAnswerServer(inputMeaning, inputReading) {
    $.ajax({
        type: "POST",
        url: "/VocabularReview/CheckAnswer",
        data: {
            Meaning: inputMeaning,
            Reading: inputReading
        },
        dataType: 'json',
        async: true,
        cache: false,
        success: function (data) {
            var bMeaning = data.meaning;
            var bReading = data["reading"];
            var bFinal = data["final"];

            var strLevel = data["levelName"];

            setMeaningResponse(bMeaning);
            console.log(bMeaning);
            setReadingResponse(bReading);
            setFinalResponse(bFinal, strLevel);
        },
        error: function (data) {
            var x;
            x = 1;
        },
        complete: function (data) {
            console.log(data);
        }
    });
}

function GetNextReview() {
    $('#see-answer').hide();
    $("#meaninginput").focus();
    $("#alert-for-success").hide();
    $("#alert-for-failure").hide();

    $.ajax({
        type: "GET",
        url: "/VocabularReview/NextReview",
        dataType: 'json',
        success: function (data) {
            var sFinish = data["finish"];
            if (sFinish === true) {
                var url = 'ReviewFinished';
                window.location.href = url;
            }
            else {
                ReloadRightAns();
                var sName = data["name"];
                var sType = data["type"];

                $('#mainDivName').removeClass("kanji-color");
                $('#mainDivName').removeClass("word-color");
                $('#mainDivName').removeClass("radical-color");

                $('#mainDivName').addClass(sType + "-color");
                $('#mainDivName').text(sName);

                if (sType == "radical") {
                    HideWanakanaReading(true);
                } else HideWanakanaReading(false);
            }
        },
        error: function (data) {
            toastr.error("?");
        }
    });

}

function ReloadRightAns() {
    $.ajax({
        type: "Get",
        url: "/VocabularReview/CurrentItem"
    }).done(function (partialViewResult) {
        $("#see-answer").html(partialViewResult);
    });
}

function HideWanakanaReading(hideReading) {
    if (hideReading != "False" && hideReading != false) {
        $("#div-reading-wanakana").hide();
    }
    else {
        //$('#div-reading-wanakana').css('display', 'normal');
        var element = document.getElementById('div-reading-wanakana');
        $("#div-reading-wanakana").show();

        element.style.display = "flex";
    }
}

function setMeaningResponse(answer) {
    if (answer == true || answer == "True") {
        $("#meaning-result").text("🠝");
        $("#meaning-result").addClass("arrow-up");
    } else {
        $("#meaning-result").text("🠟");
        $("#meaning-result").addClass("arrow-down");
    }
}

function setReadingResponse(answer) {
    if (answer == true || answer == "True") {
        $("#reading-result").text("🠝");
        $("#reading-result").addClass("arrow-up");
    } else {
        $("#reading-result").text("🠟");
        $("#reading-result").addClass("arrow-down");
    }
}

function setFinalResponse(answer, strLevel) {
    if (answer == true || answer == "True") {
        $("#alert-for-success").show();
        $("#level-name-success").text(strLevel);

        var element = document.getElementById('alert-for-success');
        element.style.display = "flex";
    } else {
        $("#alert-for-failure").show();
        $("#level-name-failure").text(strLevel);

        var element = document.getElementById('alert-for-failure');
        element.style.display = "flex";
    }
}

function ClearResponseBoxes() {
    $("#reading-result").text("");
    $("#reading-result").removeClass("arrow-up");
    $("#reading-result").removeClass("arrow-down");

    $("#meaning-result").text("");
    $("#meaning-result").removeClass("arrow-up");
    $("#meaning-result").removeClass("arrow-down");
}
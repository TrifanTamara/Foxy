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
            $('#see-answer').show();
        }
    } else {
        $('#eye-button').tooltip('disable');
        toastr.warning("You need to submit your answer first before see the right answer.");
    }
}

function validateForm(form) {
    if ($('#submitButton').is(':visible')) {
        var result = true;
        var reading = form.wanakanainput.value;
        var meaning = form.meaninginput.value;

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

        return false;
    } else {
        $('#submitButton').show();
        $('#nextButton').hide();
        $("#eye-button").addClass("inactive-button");
        $('#eye-button').tooltip('disable');
        $('#eye-button').tooltip('hide');
        ClearResponseBoxes();

        $("#meaninginput").value = "";
        $("#wanakanainput").value = "";

        document.getElementById('meaninginput').readOnly = false;
        document.getElementById('wanakanainput').readOnly = false;

        GetNextReview();
        ReloadRightAns();

        return false;
    }
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
        success: function (data) {
            var bMeaning = data["Meaning"];
            var bReading = data["Reading"];
            var bFinal = data["Final"];
            
            var strLevel = data["LevelName"];
            
            setResponse("#reading-result", bMeaning);
            setResponse("#meaning-result", bReading);
        },
        error: function (data) {
            toastr.error("?");
        }
    })
}

function GetNextReview() {
    $.ajax({
        type: "GET",
        url: "/VocabularReview/NextReview",
        dataType: 'json',
        success: function (data) {
            var sName = data["name"];
            var sType = data["type"];

            $('#mainDivName').removeClass("kanji-color");
            $('#mainDivName').removeClass("word-color");
            $('#mainDivName').removeClass("radical-color");

            $('#mainDivName').addClass(sType + "-color");
            $('#mainDivName').text(sName);

        },
        error: function (data) {
            toastr.error("?");
        }
    })
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
    if (hideReading != "False") {
        $("#div-reading-wanakana").hide();
    }
    else {
        $("#div-reading-wanakana").show();
    }
}

function setResponse(elementId, answer) {
    if (answer) {
        $(elementId).text("🠝");
        $(elementId).addClass("arrow-up");
    } else {
        $(elementId).text("🠟");
        $(elementId).addClass("arrow-down");
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
var boxCheckable = true;
var answers = [];
var paramsToSubmit = [];

jQuery(document).ready(function () {
    $('.my-check-icon').each(function () {
        this.style.display = "none";
    });
});

class QuestToSubmit {
    constructor(Id, Answer) {
        this.Id = Id;
        this.Answer = Answer;
    }
}

class SubmitList {
    constructor(ListQ, FormularId) {
        this.ListQ = ListQ;
        this.FormularId = FormularId;
    }
}

function isHidden(el) {
    var style = window.getComputedStyle(el);
    return (style.display === 'none')
}

function CheckBox(qi, ai) {
    if (boxCheckable === true) {
        var elementBox = document.getElementById("icon" + qi + "-" + ai);
        if (isHidden(elementBox)) {
            elementBox.style.display = "inline-block";
        } else {
            elementBox.style.display = "none";
        }
    }
}

function BoxIsChecked(qi, ai) {
    var elementBox = document.getElementById("icon" + qi + "-" + ai);
    if (isHidden(elementBox)) {
        return false;
    }
    return true;
}

function ShowAnswer(qi, ai) {
    var element = document.getElementById("answer-response-" + qi + "-" + ai);
    element.style.display = "inline-block";
}

function CheckAnswers(question) {
    var userResponse = [];
    for (var i = 0; i < question.answers.length; i++) {
        if (BoxIsChecked(question.number, i)) {
            userResponse.push("True");
        } else {
            userResponse.push("False");
        }
    }

    var generalResponse = true;
    for (var i = 0; i < question.answers.length; i++) {
        if (question.answers[i] !== userResponse[i]) {
            generalResponse = false;
        }
        if (question.answers[i] === "True" && userResponse[i] === "True") {
            $("#button" + question.number + "-" + i).addClass("right-answer");
            ShowAnswer(question.number, i);
        }

        if (question.answers[i] === "True" && userResponse[i] === "False") {
            var elementBox = document.getElementById("icon" + question.number + "-" + i);
            elementBox.style.display = "none";
            ShowAnswer(question.number, i);
        }

        if (userResponse[i] === "True" && generalResponse === false) {
            $("#button" + question.number + "-" + i).addClass("wrong-answer");
            ShowAnswer(question.number, i);
            var elementBox = document.getElementById("icon" + question.number + "-" + i);
            elementBox.style.display = "none";
        }
    }

    if (generalResponse === true) {
        $("#question-" + question.number).addClass("right-question");
    } else {
        $("#question-" + question.number).addClass("wrong-question");
    }

    return generalResponse;
}

function SubmitAnswers() {
    boxCheckable = false;
    var arrayLength = questionModels.length;
    var nrRight = 0;
    paramsToSubmit = [];
    for (var i = 0; i < arrayLength; i++) {
        var value = CheckAnswers(questionModels[i]);
        if (value === true) {
            nrRight += 1;
        }
        paramsToSubmit.push(new QuestToSubmit(questionModels[i].id, value));
    }
    console.log(questionModels);
    console.log(paramsToSubmit);
    $("#submitAnswersBtn").hide();
    $("#rightAnswerDiv").text(nrRight);

    var element = document.getElementById("resultDiv");
    element.style.display = "inline-block";
    var elementB = document.getElementById("anotherTestBtn");
    elementB.style.display = "inline-block";

    var sumbitList = new SubmitList(paramsToSubmit, formularId);
    var x = JSON.stringify(sumbitList);
    console.log("final"+x);
    $.ajax({
        type: "POST",
        url: "/Formular/AnswerQuestions",
        contentType: "application/json",
        data: JSON.stringify(sumbitList)
    });
}

function ReloadTest() {
    location.reload();
}


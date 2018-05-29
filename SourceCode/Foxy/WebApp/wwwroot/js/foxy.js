jQuery(document).ready(function () {
    jQuery('[data-toggle="tooltip"]').tooltip();
    jQuery('[data-toggle="popover"]').popover();

    jQuery('.popover-dismiss').popover({
        trigger: 'focus'
    })

    $('#input-reading').hide();
    $('#input-meaning').hide();
});


function RadicalCircliful(p) {
    $("#circle-radical").circliful({
        animation: 1,
        animationStep: 6,
        foregroundBorderWidth: 5,
        backgroundBorderWidth: 1,
        foregroundColor: '#96b9d7',
        percent: p
    });
}

function KanjiCircliful(p) {
    $("#circle-kanji").circliful({
        animation: 1,
        animationStep: 6,
        foregroundBorderWidth: 5,
        backgroundBorderWidth: 1,
        foregroundColor: '#717c9a',
        percent: p
    });
}

function WordCircliful(p) {
    $("#circle-word").circliful({
        animation: 1,
        animationStep: 6,
        foregroundBorderWidth: 5,
        backgroundBorderWidth: 1,
        foregroundColor: '#a77389',
        percent: p
    });
}

function playSound() {
    var audio = document.getElementById("audio");
    audio.play();
}

function RefreshReadingDiv(vId) {
    var node = document.getElementById('readingUserInput');
    $("#reading-note").text(node.value);

    $('#input-reading').hide();
    $('#editIconReading').show();

    $.ajax({
        type: "POST",
        url: "/vocabular/update/readingNote",
        data: {
            VocabularId: vId,
            NewContent: node.value
        },
        dataType: 'json'
    })
}

function ShowReadingInput() {
    $('#input-reading').show();
    $('#editIconReading').hide();
}

function ShowMeaningInput() {
    $('#input-meaning').show();
    $('#editIconMeaning').hide();
}


function RefreshDiv() {
    $('#input-reading').hide();
    $('#editIconReading').show();
}

function HideMeaningDiv() {
    $('#input-meaning').hide();
    $('#editIconMeaning').show();
}

function RefreshMeaningDiv(vId) {
    var node = document.getElementById('meaningUserInput');
    var strMM = node.value;
    $("#meaning-note").text(strMM);

    $('#input-meaning').hide();
    $('#editIconMeaning').show();

    $.ajax({
        type: "POST",
        url: "/vocabular/update/meaningNote",
        data: {
            VocabularId: vId,
            NewContent: strMM
        },
        dataType: 'json'
    })
}

function favoriteChanged(vId, myVal) {
    var checkedValue = $('#toggle-heart').val();
   
    $.ajax({
        type: "POST",
        url: "/vocabular/update/Favorite",
        data: {
            VocabularId: vId
        },
        dataType: 'json',
        success: function (data) {
            if (data) {
                toastr.success("Item added to favorite list!")
            } else {
                toastr.warning("Item removed from favorite!")
            }
        }
    })
}

function CheckHeart() {
    var x = 0;
    $("#toggle-heart").attr("checked", "checked");
    if ($("#toggle-heart").val=="on"){
        x = 1;
    }
    x = -3;
}
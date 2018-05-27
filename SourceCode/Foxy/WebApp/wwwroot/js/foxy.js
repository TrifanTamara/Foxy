jQuery(document).ready(function () {
    jQuery('[data-toggle="tooltip"]').tooltip();
    jQuery('[data-toggle="popover"]').popover();

    jQuery('.popover-dismiss').popover({
        trigger: 'focus'
    })

    $('#input-reading').hide();
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

function RefreshReadingDiv() {
    var node = document.getElementById('readingUserInput');
    $("#reading-note").text(node.value);

    $('#input-reading').hide();
    $('#editIconReading').show();
}

function ShowReadingInput() {
    $('#input-reading').show();
    $('#editIconReading').hide();
}

function RefreshDiv() {
    $('#input-reading').hide();
    $('#editIconReading').show();
}
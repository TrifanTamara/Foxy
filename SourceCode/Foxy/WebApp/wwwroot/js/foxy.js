jQuery(document).ready(function () {
    jQuery('[data-toggle="tooltip"]').tooltip();
    jQuery('[data-toggle="popover"]').popover()
    jQuery('.popover-dismiss').popover({
        trigger: 'focus'
    })
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
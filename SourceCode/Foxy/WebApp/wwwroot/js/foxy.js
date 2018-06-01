﻿jQuery(document).ready(function () {
    jQuery('[data-toggle="tooltip"]').tooltip();
    jQuery('[data-toggle="popover"]').popover();

    jQuery('.popover-dismiss').popover({
        trigger: 'focus'
    })

    $('#input-reading').hide();
    $('#input-meaning').hide();
    $('#input-synonyms').hide();

    $('.carousel').carousel({
        interval: false
    });

    $('#navigateBackItem').click(function (e) {
        e.preventDefault();
        NavigatePreviousItem();
    });
    $('#navigateAheadMeaning').click(function (e) {
        e.preventDefault();
        NavigateToMeaning();
    });
    $('#navigateBackStructure').click(function (e) {
        e.preventDefault();
        NavigateToStructure();
    });
    $('#navigateAheadReading').click(function (e) {
        e.preventDefault();
        NavigatToReading();
    });
    $('#navigateBackMeaning').click(function (e) {
        e.preventDefault();
        NavigateToMeaning();
    });
    $('#navigateNextItem').click(function (e) {
        e.preventDefault();
        NavigatNextItem();
    });


    $(document).keydown(function (e) {

        if (e.keyCode == '37') {
            // left arrow   
            e.preventDefault();
            NavigateBack();
        } else if (event.keyCode == '39') {
            // right arrow
            e.preventDefault();
            NavigateNext();
        }
    });
});


function RemoveActiveFromNavTabs() {
    $('#nav-structure').removeClass('active');
    $('#nav-meaning').removeClass('active');
    $('#nav-reading').removeClass('active');
    $('#nav-structure').removeClass('show');
    $('#nav-meaning').removeClass('show');
    $('#nav-reading').removeClass('show');
    $('#nav-structure-tab').removeClass('active');
    $('#nav-meaning-tab').removeClass('active');
    $('#nav-reading-tab').removeClass('active');
}

function NavigatePreviousItem() {
    $.ajax({
        type: "Get",
        url: "/VocabularLesson/PreviousItem",
        dataType: 'json',
        success: function (data) {
            var name = data["name"];
            var meaning = data["meaning"];
            var activeIndex = data["activeIndex"];
            var activeReview = data["activeReview"];

            $('#mainDivName').text(name);
            $('#mainDivMeaning').text(meaning);
        },
        error: function (data) {
            toastr.error("?");
            var x = 1;
        }
    });
    ReloadAllTabs();
    NavigateToStructure();
}

function ReloadAllTabs() {
    var t0 = performance.now();
    $.ajax({
        type: "Get",
        url: "/VocabularLesson/StructureTab"
    }).done(function (partialViewResult) {
        $("#middle-div-structure").html(partialViewResult);
    });

    $.ajax({
        type: "Get",
        url: "/VocabularLesson/MeaningTab"
    }).done(function (partialViewResult) {
        $("#middle-div-meaning").html(partialViewResult);
    });

    $.ajax({
        type: "Get",
        url: "/VocabularLesson/ReadingTab"
    }).done(function (partialViewResult) {
        $("#middle-div-reading").html(partialViewResult);
        });

    var t1 = performance.now();
    console.log("Call to doSomething took " + (t1 - t0) + " milliseconds.")
}

function NavigatNextItem() {
    $.ajax({
        type: "Get",
        url: "/VocabularLesson/NextItem",
        dataType: 'json',
        success: function (data) {
            var name = data["name"];
            var meaning = data["meaning"];
            var activeIndex = data["activeIndex"];
            var activeReview = data["activeReview"];
            var showModal = data["showModal"];

            $('#mainDivName').text(name);
            $('#mainDivMeaning').text(meaning);
        },
        error: function (data) {
            toastr.error("?");
            var x = 1;
        }
    });
    ReloadAllTabs();
    NavigateToStructure();
}

function NavigateToStructure() {
    RemoveActiveFromNavTabs();
    $('#nav-structure').addClass('active');
    $('#nav-structure-tab').addClass('active');
    $('#nav-structure').addClass('show');
}
function NavigateToMeaning() {
    RemoveActiveFromNavTabs();
    $('#nav-meaning').addClass('active');
    $('#nav-meaning-tab').addClass('active');
    $('#nav-meaning').addClass('show');
}
function NavigatToReading() {
    RemoveActiveFromNavTabs();
    $('#nav-reading').addClass('active');
    $('#nav-reading-tab').addClass('active');
    $('#nav-reading').addClass('show');
}


function NavigateBack() {
    if ($('#nav-structure-tab').hasClass('active')) {
        NavigatePreviousItem();
    } else if ($('#nav-meaning-tab').hasClass('active')) {
        NavigateToStructure();
    } else if ($('#nav-reading-tab').hasClass('active')) {
        NavigateToMeaning();
    }
}
function NavigateNext() {
    if ($('#nav-structure-tab').hasClass('active')) {
        NavigateToMeaning();
    } else if ($('#nav-meaning-tab').hasClass('active')) {
        NavigatToReading();
    } else if ($('#nav-reading-tab').hasClass('active')) {
        NavigatNextItem();
    }
}

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

    toastr.success("Meaning note updated!");

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
    if ($("#toggle-heart").val == "on") {
        x = 1;
    }
    x = -3;
}

function GetSynonymsNumber() {
    var elemNr = 0;
    var str;
    for (var i = 0; i < 5; i++) {
        str = "#synonym" + i;
        if ($(str).is(":visible")) elemNr += 1;
    }
    var f = 1;
    return elemNr;
}

function AddSynoymClicked() {
    if (GetSynonymsNumber() >= 5) {
        toastr.error("You can't add more than 5 synonyms!")
    } else {
        $('#add-syn-button').hide();
        $('#input-synonyms').show();
    }
}

function AddSynonim(vId) {
    $('#add-syn-button').show();
    $('#input-synonyms').hide();

    var node = document.getElementById('synInput');
    var strMM = node.value;

    $.ajax({
        type: "POST",
        url: "/vocabular/addSynonym",
        data: {
            VocabularId: vId,
            NewContent: strMM
        },
        dataType: 'json',
        success: function (data) {
            //var json = JSON.parse(data);
            var str = data["synonyms"];
            var list = str.split(";");
            var arrayLength = list.length;
            var txt = "";
            var str;
            var sDiv;
            for (var i = 0; i < arrayLength; i++) {
                str = "#synonym" + i;
                $(str).text(list[i]);
                sDiv = "#divSyn" + i;
                $(sDiv).show();
            }
            for (var i = arrayLength; i < 5; i++) {
                str = "#synonym" + i;
                sDiv = "#divSyn" + i;
                $(sDiv).hide();
            }
            toastr.success("New synonym added!");
        },
        error: function (data) {
            toastr.error("?");
            var x = 1;
        }
    })

}

function CloseInputSyn() {
    $('#add-syn-button').show();
    $('#input-synonyms').hide();
}

function HideSynonyms(index) {
    var str;
    for (var i = index; i < 5; i++) {
        str = "#divSyn" + i;
        $(str).hide();
    }
}

function RemoveSynonym(vId, index) {
    var node = document.getElementById('synInput');
    var strMM = node.value;

    $.ajax({
        type: "POST",
        url: "/vocabular/removeSynonym",
        data: {
            VocabularId: vId,
            Index: index
        },
        dataType: 'json',
        success: function (data) {
            //var json = JSON.parse(data);
            var str = data["synonyms"];
            if (str == "") HideSynonyms(0);
            else {
                var list = str.split(";");
                var arrayLength = list.length;
                var txt = "";
                var str;
                var sDiv;
                for (var i = 0; i < arrayLength; i++) {
                    str = "#synonym" + i;
                    $(str).text(list[i]);
                    sDiv = "#divSyn" + i;
                    $(sDiv).show();
                }
                for (var i = arrayLength; i < 5; i++) {
                    str = "#synonym" + i;
                    sDiv = "#divSyn" + i;
                    $(sDiv).hide();
                }
            }
            toastr.warning("Synonym removed");

        },
        error: function (data) {
            toastr.error("?");
            var x = 1;
        }
    })
}
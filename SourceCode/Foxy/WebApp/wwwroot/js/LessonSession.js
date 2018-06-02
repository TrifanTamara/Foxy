var globalIndex = 0;
var lessonNumber;

jQuery(document).ready(function () {
    LessonHideInputs();
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
        NavigateNextItem();
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

function HideAllTabs(nr) {
    for (var i = 0; i < nr; i++) {
        $("#meaning-div-" + i).hide();
        $("#reading-div-" + i).hide();
        $("#structure-div-" + i).hide();
    }
}

function ShowTabs(index) {
    globalIndex = index;
    //$("#meaning-div-" + index).show();
    //$("#reading-div-" + index).show();
    $("#structure-div-" + index).show();
}

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

function ReplaceContent() {
    var strName = "scheme-structure-div-" + globalIndex;

    var content1 = document.getElementById(strName).innerHTML;
    var theDivStructure = document.getElementById("middle-div-structure");
    theDivStructure.innerHTML = content1;

    var content2 = document.getElementById("scheme-meaning-div-" + globalIndex).innerHTML;
    var theDivMeaning = document.getElementById("middle-div-meaning");
    theDivMeaning.innerHTML = content2;

    var content3 = document.getElementById("scheme-reading-div-" + globalIndex).innerHTML;
    var theDivReading = document.getElementById("middle-div-reading");
    theDivReading.innerHTML = content3;

    var content4 = document.getElementById("scheme-huge-" + globalIndex).innerHTML;
    var theHuge = document.getElementById("huge-text-div");
    theHuge.innerHTML = content4;
}

function NavigatePreviousItem() {
    if (globalIndex > 0) globalIndex -= 1;
    ReplaceContent();
    NavigateToStructure();
    LessonHideInputs();
    LessonHideShowSynonyms();
}

function NavigateNextItem() {
    if (globalIndex < lessonNumber - 1) globalIndex += 1;

    ReplaceContent();

    if (globalIndex < lessonNumber - 1) NavigateToStructure();
    LessonHideInputs();
    LessonHideShowSynonyms();
}

function NavigateToStructure() {
    RemoveActiveFromNavTabs();
    $('#nav-structure').addClass('active');
    $('#nav-structure-tab').addClass('active');

    $('#nav-structure').addClass('show');
    HideAllTabs();
    $('#structure-div-' + globalIndex).show;
}
function NavigateToMeaning() {
    RemoveActiveFromNavTabs();
    $('#nav-meaning').addClass('active');
    $('#nav-meaning-tab').addClass('active');

    $('#nav-meaning').addClass('show');
    HideAllTabs();
    var str = '#meaning-div-' + globalIndex;
    $(str).show;
}
function NavigatToReading() {
    RemoveActiveFromNavTabs();
    $('#nav-reading').addClass('active');
    $('#nav-reading-tab').addClass('active');

    $('#nav-reading').addClass('show');
    HideAllTabs();
    $('#reading-div-' + globalIndex).addClass('show');
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
        NavigateNextItem();
    }
}


function LessonRefreshMeaningDiv(vId) {
    var node;
    $('.meaningUserInput').each(function () {
        if ($(this).parents('div#middle-div-meaning').length) {
            node = this;
        }
    });
    var strMM = node.value;
    $('.meaning-note').each(function () {
        if ($(this).parents('div#middle-div-meaning').length) {
            $(this).text(strMM);
        }
        var parentStr = 'div#scheme-meaning-div-' + globalIndex;
        if ($(this).parents(parentStr).length) {
            $(this).text(strMM);
        }
    });
    LessonHideMeaningDiv();

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

function LessonShowMeaningInput() {
    $('.input-meaning').each(function () {
        if ($(this).parents('div#middle-div-meaning').length) {
            $(this).show();
        }
    });

    $('.editIconMeaning').hide();
}


function LessonHideMeaningDiv() {
    $('.input-meaning').hide();

    $('.editIconMeaning').each(function () {
        if ($(this).parents('div#middle-div-meaning').length) {
            $(this).show();
        }
    });
}


function LessonHideInputs() {
    $('.input-reading').hide();
    $('.input-meaning').hide();
    $('.input-synonyms').hide();
}

function LessonRefreshReadingDiv(vId) {
    var node;
    $('.readingUserInput').each(function () {
        if ($(this).parents('div#middle-div-reading').length) {
            node = this;
        }
    });
    var strMM = node.value;
    $('.reading-note').each(function () {
        if ($(this).parents('div#middle-div-reading').length) {
            $(this).text(strMM);
        }
        var parentStr = 'div#scheme-reading-div-' + globalIndex;
        if ($(this).parents(parentStr).length) {
            $(this).text(strMM);
        }
    });

    toastr.success("Reading note updated!");
    LessonHideReadingDiv();

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

function LessonShowReadingInput() {
    $('.input-reading').each(function () {
        if ($(this).parents('div#middle-div-reading').length) {
            $(this).show();
        }
    });

    $('.editIconReading').hide();
}

function LessonHideReadingDiv() {
    $('.editIconReading').hide();

    $('.input-reading').each(function () {
        if ($(this).parents('div#middle-div-reading').length) {
            $(this).show();
        }
    });
}

function LessonRemoveSynonym(vId, index) {
    $.ajax({
        type: "POST",
        url: "/vocabular/removeSynonym",
        data: {
            VocabularId: vId,
            Index: index
        },
        dataType: 'json',
        success: function (data) {
            var str = data["synonyms"];
            //?????????????
            if (str == "") LessonHideShowSynonyms();
            else {
                var list = str.split(";");
                var arrayLength = list.length;
                var txt = "";
                var str;
                var sDiv;
                for (var i = 0; i < arrayLength; i++) {
                    str = ".synonym" + i;
                    $(str).each(function () {
                        if ($(this).parents('div#middle-div-meaning').length) {
                            $(this).text(list[i]);
                        }
                        var parentStr = 'div#scheme-meaning-div-' + globalIndex;
                        if ($(this).parents(parentStr).length) {
                            $(this).text(list[i]);
                        }
                    });
                }
                for (var i = arrayLength; i < 5; i++) {
                    str = ".synonym" + i;
                    $(str).each(function () {
                        if ($(this).parents('div#middle-div-meaning').length) {
                            $(this).text("?");
                        }
                        var parentStr = 'div#scheme-meaning-div-' + globalIndex;
                        if ($(this).parents(parentStr).length) {
                            $(this).text("?");
                        }
                    });
                }
                LessonHideShowSynonyms();
            }
            toastr.warning("Synonym removed");
        },
        error: function (data) {
            toastr.error("?");
            var x = 1;
        }
    })
}

function LessonHideShowSynonyms() {
    var str;
    var index = LessonGetSynonymsNumber();
    for (var i = 0; i < index; i++) {
        str = ".divSyn" + i;
        $(str).each(function () {
            if ($(this).parents('div#middle-div-meaning').length) {
                $(this).show();
            }
        });
    }

    for (var i = index; i < 5; i++) {
        str = ".divSyn" + i;
        $(str).hide();
    }
}

function LessonGetSynonymsNumber() {
    var elemNr = 5;
    var str;
    for (var i = 0; i < 5; i++) {
        str = ".synonym" + i;
        var x = "div#middle-div-meaning";
        $(str).each(function () {
            if ($(this).parents(x).length) {
                if (this.innerHTML == "?") elemNr -= 1;
            }
        });
    }
    return elemNr;
}

function LessonAddSynoymClicked() {
    if (LessonGetSynonymsNumber() >= 5) {
        toastr.error("You can't add more than 5 synonyms!")
    } else {
        $('.add-syn-button').hide();
        $(".input-synonyms").each(function () {
            if ($(this).parents('div#middle-div-meaning').length) {
                $(this).show();
            }
        });
    }
}

function LessonAddSynonim(vId) {
    LessonCloseInputSyn();
    var node;
    $(".synInput").each(function () {
        if ($(this).parents('div#middle-div-meaning').length) {
            node = this;
        }
    });
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
                str = ".synonym" + i;
                $(str).each(function () {
                    if ($(this).parents('div#middle-div-meaning').length) {
                        $(this).text(list[i]);
                    }
                    var parentStr = 'div#scheme-meaning-div-' + globalIndex;
                    if ($(this).parents(parentStr).length) {
                        $(this).text(list[i]);
                    }
                });
            }
            for (var i = arrayLength; i < 5; i++) {
                str = ".synonym" + i;
                $(str).each(function () {
                    if ($(this).parents('div#middle-div-meaning').length) {
                        $(this).text("?");
                    }
                    var parentStr = 'div#scheme-meaning-div-' + globalIndex;
                    if ($(this).parents(parentStr).length) {
                        $(this).text("?");
                    }
                });
            }
            LessonHideShowSynonyms();

            toastr.success("New synonym added!");
        },
        error: function (data) {
            toastr.error("?");
            var x = 1;
        }
    })

}


function LessonCloseInputSyn() {
    $('.input-synonyms').hide();
    $(".add-syn-button").hide();
    $(".add-syn-button").each(function () {
        if ($(this).parents('div#middle-div-meaning').length) {
            $(this).show();
        }
    });
}
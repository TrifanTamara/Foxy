$(document).ready(function () {
    $('.dropdown-toggle').dropdown();
    var scrollTop = 0;
    if (window.innerWidth < 768) {
        mobile();
    } else {
        maximizeNav();
    }

    $(window).scroll(function () {
        scrollTop = $(window).scrollTop();
        $('.counter').html(scrollTop);
        if (window.innerWidth >= 768) {
            maximizeNav();
            if (scrollTop >= 100) {
                minimizeNav();
            } else if (scrollTop < 100) {
                maximizeNav();
            }
        } else {
            minimizeNav();
        } 

    });

});


$(window).resize(function () {
    if (window.innerWidth < 768) {
        mobile();
    }
});

function minimizeNav() {
    HideBrand();
    $('#main-navbar').addClass('scrolled-nav');
    $('#main-navbar').removeClass('class-myNav');
}

function maximizeNav() {
    $('#main-navbar').addClass('class-myNav');
    $('#main-navbar').removeClass('scrolled-nav');
    //$('.menu-drop-main').removeClass('class-myNav');
    ShowBrand();
}

function mobile() {
    $('#main-navbar').removeClass('class-myNav');
    $('#main-navbar').removeClass('scrolled-nav');
    HideBrand();
}

function ShowBrand() {
    $("#foxy-icon").show();
    $('#foxy-title-id').removeClass('foxy-title-second');
    $('#foxy-title-id').addClass('foxy-title-first');
    $('li').addClass('big-nav-li');
    $('li').remove('small-nav-li');

    $('#levels-id').removeClass('circle-small');
    $('#levels-id').addClass('circle-big');


    $('#menuVocabLink').removeClass('text-item-small');
}

function HideBrand() {
    $("#foxy-icon").hide();
    $('#foxy-title-id').addClass('foxy-title-second');
    $('#foxy-title-id').removeClass('foxy-title-first');
    $('li').removeClass('big-nav-li');
    $('li').addClass('small-nav-li');
    
    $('#menuVocabLink').addClass('text-item-small');

    $('#levels-id').addClass('circle-small');
    $('#levels-id').removeClass('circle-big');
}
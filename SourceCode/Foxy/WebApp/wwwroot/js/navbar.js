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

    $('.div-saviour').removeClass('div-container-list-text');

    $('.liNavbar').removeClass('small-nav-li');
    $('.liNavbar').addClass('big-nav-li');

    $('.levels-id').removeClass('circle-small');
    $('.levels-id').addClass('circle-big');

    
    $('.menuVocabLink').removeClass('text-item-small');

    $('.hide-when-small').show();
}

function HideBrand() {
    $("#foxy-icon").hide();


    $('.div-saviour').addClass('div-container-list-text');

    $('#foxy-title-id').addClass('foxy-title-second');
    $('#foxy-title-id').removeClass('foxy-title-first');

    $('.liNavbar').addClass('small-nav-li');
    $('.liNavbar').removeClass('big-nav-li');
    
    $('.menuVocabLink').addClass('text-item-small');

    $('.levels-id').addClass('circle-small');
    $('.levels-id').removeClass('circle-big');

    $('.hide-when-small').hide();
}
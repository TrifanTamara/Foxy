$(document).ready(function () {
    $('.dropdown-toggle').dropdown();
    var scrollTop = 0;
    if (window.innerWidth < 768)
    {
        $('#main-navbar').removeClass('class-myNav');
        $('#main-navbar').removeClass('scrolled-nav');
    }

    $(window).scroll(function () {
        scrollTop = $(window).scrollTop();
        $('.counter').html(scrollTop);
        if (window.innerWidth >= 768) {
            $('#main-navbar').addClass('class-myNav');
            $('.menu-drop-main').removeClass('class-myNav');
            if (scrollTop >= 100) {
                $('#main-navbar').addClass('scrolled-nav');
            } else if (scrollTop < 100) {
                $('#main-navbar').removeClass('scrolled-nav');
            }
        } else {
            $('#main-navbar').removeClass('class-myNav');
        } 

    });

});


$(window).resize(function () {
    if (window.innerWidth < 768) {
        $('#main-navbar').removeClass('class-myNav');
        $('#main-navbar').removeClass('scrolled-nav');
    }
    
});
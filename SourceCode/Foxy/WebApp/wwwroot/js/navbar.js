$(document).ready(function () {
    var scrollTop = 0;
    $(window).scroll(function () {
        scrollTop = $(window).scrollTop();
        $('.counter').html(scrollTop);
        if (window.innerWidth >= 768) {
            $('#main-navbar').addClass('class-myNav');
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
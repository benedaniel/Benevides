$(function () {
    $('#slidesDestaque-').slidesjs({
        width: 628,
        height: 328,
        navigation: {
            active: false,
            effect: "slide"
        },
        play: {
            active: false,
            effect: "slide",
            interval: 20000,
            auto: true,
            swap: true,
            pauseOnHover: true
        }
    });
});
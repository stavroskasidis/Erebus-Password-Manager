var home = (function (home) {

    home.initializeMenu = function () {
        home.menu = $('#menu').easytree({
            dataUrl: "/Home/GetMenuData"
        });
    };


    return home;
}(home || {}));

$(function () {
    home.initializeMenu();
});

(function () {
    angular.module('leftmenu.module', ['cvmk.common.module']).config(config);
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('leftmenu', {
                parent: 'base',
                url: "/leftmenu",
                templateUrl: "/App/components/leftmenu/leftmenu.html",
                controller: "leftmenuController"
            }).state('createleftmenu', {
                parent: 'base',
                url: "/leftmenu/create",
                templateUrl: "/App/components/leftmenu/createleftmenu.html",
                controller: "createleftmenuController"
            }).state('editleftmenu', {
                parent: 'base',
                url: "/leftmenu/edit/:id",
                templateUrl: "/App/components/leftmenu/editleftmenu.html",
                controller: "editleftmenuController"
            });
    }
})();
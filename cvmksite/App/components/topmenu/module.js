(function () {
    angular.module('topmenu.module', ['cvmk.common.module']).config(config);
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('topmenu', {
                parent: 'base',
                url: "/topmenu",
                templateUrl: "/App/components/topmenu/topmenu.html",
                controller: "topmenuController"
            }).state('createtopmenu', {
                parent: 'base',
                url: "/topmenu/create",
                templateUrl: "/App/components/topmenu/createtopmenu.html",
                controller: "createtopmenuController"
            }).state('edittopmenu', {
                parent: 'base',
                url: "/topmenu/edit/:id",
                templateUrl: "/App/components/topmenu/edittopmenu.html",
                controller: "edittopmenuController"
            });
    }
})();
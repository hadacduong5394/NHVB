(function () {
    angular.module('floor.module', ['cvmk.common.module']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('floor', {
                parent: 'base',
                url: "/floor",
                templateUrl: "/App/components/floor/floor.html",
                controller: "floorController"
            }).state('createfloor', {
                parent: 'base',
                url: "/floor/create",
                templateUrl: "/App/components/floor/createFloor.html",
                controller: "createFloorController"
            }).state('editfloor', {
                parent: 'base',
                url: "/floor/edit/:id",
                templateUrl: "/App/components/floor/editFloor.html",
                controller: "editFloorController"
            });
    }
})();
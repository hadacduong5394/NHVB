(function () {
    angular.module('groupproduct.module', ['cvmk.common.module']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('groupproduct', {
                parent: 'base',
                url: "/groupproduct",
                templateUrl: "/App/components/groupproduct/groupproduct.html",
                controller: "groupproductController"
            }).state('creategroupproduct', {
                parent: 'base',
                url: "/groupproduct/create",
                templateUrl: "/App/components/groupproduct/createGroupproduct.html",
                controller: "createGroupproductController"
            }).state('editgroupproduct', {
                parent: 'base',
                url: "/groupproduct/edit/:id",
                templateUrl: "/App/components/groupproduct/editGroupproduct.html",
                controller: "editGroupproductController"
            });
    }
})();
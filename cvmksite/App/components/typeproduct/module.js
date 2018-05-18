(function () {
    angular.module('typeproduct.module', ['cvmk.common.module']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('typeproduct', {
                parent: 'base',
                url: "/typeproduct",
                templateUrl: "/App/components/typeproduct/typeproduct.html",
                controller: "typeproductController"
            }).state('createtypeproduct', {
                parent: 'base',
                url: "/groupproduct/create",
                templateUrl: "/App/components/typeproduct/createTypeproduct.html",
                controller: "createTypeproductController"
            }).state('edittypeproduct', {
                parent: 'base',
                url: "/typeproduct/edit/:id",
                templateUrl: "/App/components/typeproduct/editTypeproduct.html",
                controller: "editTypeproductController"
            });
    }
})();
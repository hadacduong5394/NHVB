(function () {
    angular.module('menuproduct.module', ['cvmk.common.module']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('menuproduct', {
                parent: 'base',
                url: "/menuproduct",
                templateUrl: "/App/components/menuproduct/menuproduct.html",
                controller: "menuproductController"
            });
    }
})();
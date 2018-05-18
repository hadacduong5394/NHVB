(function () {
    angular.module('order.module', ['cvmk.common.module']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('order', {
                parent: 'base',
                url: "/order",
                templateUrl: "/App/components/order/order.html",
                controller: "orderController"
            });
    }
})();
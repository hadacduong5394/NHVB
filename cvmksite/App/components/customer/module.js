(function () {
    angular.module('customer.module', ['cvmk.common.module']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('customer', {
                parent: 'base',
                url: "/customer",
                templateUrl: "/App/components/customer/customer.html",
                controller: "customerController"
            }).state('createcustomer', {
                parent: 'base',
                url: "/customer/create",
                templateUrl: "/App/components/customer/createCustomer.html",
                controller: "createCustomerController"
            }).state('editcustomer', {
                parent: 'base',
                url: "/customer/edit/:id",
                templateUrl: "/App/components/customer/editCustomer.html",
                controller: "editCustomerController"
            });
    }
})();
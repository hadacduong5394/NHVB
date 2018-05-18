(function () {
    angular.module('supplier.module', ['cvmk.common.module']).config(config);
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('supplier', {
                parent: 'base',
                url: "/supplier",
                templateUrl: "/App/components/supplier/supplier.html",
                controller: "supplierController"
            }).state('createsupplier', {
                parent: 'base',
                url: "/supplier/create",
                templateUrl: "/App/components/supplier/createSupplier.html",
                controller: "createSupplierController"
            }).state('editsupplier', {
                parent: 'base',
                url: "/supplier/edit/:id",
                templateUrl: "/App/components/supplier/editSupplier.html",
                controller: "editSupplierController"
            });
    }
})();
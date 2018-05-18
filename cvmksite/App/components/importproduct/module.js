(function () {
    angular.module('importproduct.module', ['cvmk.common.module']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('importproduct', {
                parent: 'base',
                url: "/importproduct",
                templateUrl: "/App/components/importproduct/importProduct.html",
                controller: "importProductController"
            }).state('createimportproduct', {
                parent: 'base',
                url: "/importproduct/create",
                templateUrl: "/App/components/importproduct/createImportProduct.html",
                controller: "createImportProductController"
            }).state('editimportproduct', {
                parent: 'base',
                url: "/importproduct/edit/:id",
                templateUrl: "/App/components/importproduct/editImportProduct.html",
                controller: "editImportProductController"
            });
    }
})();
(function () {
    angular.module('product.module', ['cvmk.common.module', 'ngCkeditor']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('product', {
                parent: 'base',
                url: "/product",
                templateUrl: "/App/components/product/product.html",
                controller: "productController"
            }).state('createproduct', {
                parent: 'base',
                url: "/product/create",
                templateUrl: "/App/components/product/createProduct.html",
                controller: "createProductController"
            }).state('editproduct', {
                parent: 'base',
                url: "/product/edit/:id",
                templateUrl: "/App/components/product/editProduct.html",
                controller: "editProductController"
            });
    }
})();
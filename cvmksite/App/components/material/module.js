(function () {
    angular.module('material.module', ['cvmk.common.module']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('material', {
                parent: 'base',
                url: "/material",
                templateUrl: "/App/components/material/material.html",
                controller: "materialController"
            }).state('creatematerial', {
                parent: 'base',
                url: "/material/creatematerial",
                templateUrl: "/App/components/material/createMaterial.html",
                controller: "createMaterialController"
            }).state('editmaterial', {
                parent: 'base',
                url: "/material/editmaterial/:id",
                templateUrl: "/App/components/material/editMaterial.html",
                controller: "editMaterialController"
            });
    }
})();
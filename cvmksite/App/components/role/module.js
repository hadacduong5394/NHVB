(function () {
    angular.module('role.module', ['cvmk.common.module']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('role', {
                parent: 'base',
                url: "/role",
                templateUrl: "/App/components/role/role.html",
                controller: "roleController"
            }).state('createrole', {
                parent: 'base',
                url: "/role/create",
                templateUrl: "/App/components/role/createRole.html",
                controller: "createRoleController"
            }).state('editrole', {
                parent: 'base',
                url: "/role/edit/:id",
                templateUrl: "/App/components/role/editRole.html",
                controller: "editRoleController"
            });
    }
})();
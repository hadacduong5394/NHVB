(function () {
    angular.module('user.module', ['cvmk.common.module']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('user', {
                parent: 'base',
                url: "/user",
                templateUrl: "/App/components/user/user.html",
                controller: "userController"
            }).state('createuser', {
                parent: 'base',
                url: "/user/createuser",
                templateUrl: "/App/components/user/createUser.html",
                controller: "createuserController"
            }).state('edituser', {
                parent: 'base',
                url: "/user/edituser/:username",
                templateUrl: "/App/components/user/editUser.html",
                controller: "edituserController"
            });
    }
})();
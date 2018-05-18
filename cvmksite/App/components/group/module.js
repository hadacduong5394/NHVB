(function () {
    angular.module('group.module', ['cvmk.common.module']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('group', {
                parent: 'base',
                url: "/group",
                templateUrl: "/App/components/group/group.html",
                controller: "groupController"
            }).state('creategroup', {
                parent: 'base',
                url: "/group/creategroup",
                templateUrl: "/App/components/group/createGroup.html",
                controller: "createGroupController"
            }).state('editgroup', {
                parent: 'base',
                url: "/group/editgroup/:id",
                templateUrl: "/App/components/group/editGroup.html",
                controller: "editGroupController"
            });
    }
})();
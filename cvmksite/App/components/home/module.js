(function () {
    angular.module('home.module', ['cvmk.common.module']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('home', {
                parent: 'base',
                url: "/group",
                templateUrl: "/App/components/group/group.html",
                controller: "groupController"
            })
    }
})();
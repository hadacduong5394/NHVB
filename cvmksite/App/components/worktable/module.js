(function () {
    angular.module('worktable.module', ['cvmk.common.module']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('worktable', {
                parent: 'base',
                url: "/worktable",
                templateUrl: "/App/components/worktable/worktable.html",
                controller: "worktableController"
            });
    }
})();
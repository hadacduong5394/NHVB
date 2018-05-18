(function () {
    angular.module('companyinfo.module', ['cvmk.common.module']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('companyinfo', {
                parent: 'base',
                url: "/companyinfo",
                templateUrl: "/App/components/companyinfo/companyinfo.html",
                controller: "companyinfoController"
            }).state('createcompanyinfo', {
                parent: 'base',
                url: "/companyinfo/create",
                templateUrl: "/App/components/companyinfo/createCompanyinfo.html",
                controller: "createCompanyinfoController"
            }).state('editcompanyinfo', {
                parent: 'base',
                url: "/companyinfo/edit/:id",
                templateUrl: "/App/components/companyinfo/editCompanyinfo.html",
                controller: "editCompanyinfoController"
            });
    }
})();
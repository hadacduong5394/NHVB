(function () {
    angular.module('cvmk.common.module', ['ui.router', 'angular-loading-bar', 'ngBootbox', 'LocalStorageModule']).config(config);
    config.$inject = ['cfpLoadingBarProvider'];
    function config(cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeSpinner = true;
    }
})();
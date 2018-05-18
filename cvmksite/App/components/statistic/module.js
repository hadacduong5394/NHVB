(function () {
    angular.module('statistic.module', ['cvmk.common.module', 'chart.js']).config(config);
	config.$inject = ['$stateProvider', '$urlRouterProvider'];
	function config($stateProvider, $urlRouterProvider) {
		$stateProvider
            .state('revenues', {
            	parent: 'base',
            	url: "/statistic/revenues",
            	templateUrl: "/App/components/statistic/statisticRevenues.html",
            	controller: "statisticRevenuesController"
            }).state('imports', {
                parent: 'base',
                url: "/statistic/imports",
                templateUrl: "/App/components/statistic/statisticImports.html",
                controller: "statisticImportsController"
            });
	}
})();
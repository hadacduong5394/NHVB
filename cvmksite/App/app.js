(function () {
    angular.module('cvmk.app',
        ['cvmk.common.module',
        'home.module',
        'user.module',
        'role.module',
        'leftmenu.module',
        'topmenu.module',
        "supplier.module",
        'customer.module',
        'floor.module',
        'room.module',
        'product.module',
        'importproduct.module',
        'worktable.module',
        'menuproduct.module',
        'order.module',
        'material.module',
        'statistic.module',
        'groupproduct.module',
        'typeproduct.module',
        'companyinfo.module',
        'group.module'])
        .config(config)
        .config(configAuthentication);
    config.$inject = ['$stateProvider', '$urlRouterProvider', '$httpProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('base', {
                        url: '',
                        templateUrl: '/App/shared/_layout/baseView.html',
                        abstract: true
                    }).state('login', {
                        url: "/login",
                        templateUrl: "/App/components/login/login.html",
                        controller: "loginController"
                    }).state('lostpassword', {
                        url: "/lostpassword",
                        templateUrl: "/App/components/lostpassword/lostpassword.html",
                        controller: "lostpasswordController"
                    });
        $urlRouterProvider.otherwise(function ($injector) {
            var $state = $injector.get('$state');
            $state.go('login');
        });
    }
    function configAuthentication($httpProvider) {
        $httpProvider.interceptors.push(function ($q, $location) {
            return {
                request: function (config) {

                    return config;
                },
                requestError: function (rejection) {
                    return $q.reject(rejection);
                },
                response: function (response) {
                    if (response.status == "401") {
                        $location.path('/login');
                    }
                    //the same response/modified/or a new one need to be returned.
                    return response;
                },
                responseError: function (rejection) {
                    if (rejection.status == "401" && (rejection.data != '')) {
                        $location.path('/login');
                    }
                    return $q.reject(rejection);
                }
            };
        });
    }
})();
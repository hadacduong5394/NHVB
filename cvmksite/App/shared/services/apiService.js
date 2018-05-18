(function (app) {
    app.factory('apiService', apiService);

    apiService.$inject = ['$http', 'cfpLoadingBar', 'notificationService', 'authenticationService', '$state'];

    function apiService($http, cfpLoadingBar, notificationService, authenticationService, $state) {
        return {
            get: get,
            post: post,
            put: put,
            del: del
        }

        function del(url, data, success, failure) {
            cfpLoadingBar.start();
            authenticationService.setHeader();
            $http.delete(url, data).then(function (result) {
                success(result);
                cfpLoadingBar.complete();
            }, function (error) {
                if (error.status === 401) {
                    notificationService.displayError('Authenticate is required.');
                }
                else if (failure != null) {
                    failure(error);
                }
                cfpLoadingBar.complete();
            });
        }

        function post(url, data, success, failure) {
            cfpLoadingBar.start();
            authenticationService.setHeader();
            $http.post(url, data).then(function (result) {
                success(result);
                cfpLoadingBar.complete();
            }, function (error) {
                if (error.status === 401) {
                    notificationService.displayError('Authenticate is required.');
                }
                else if (failure != null) {
                    failure(error);
                }
                cfpLoadingBar.complete();
            });
        }

        function put(url, data, success, failure) {
            cfpLoadingBar.start();
            authenticationService.setHeader();
            $http.put(url, data).then(function (result) {
                success(result);
                cfpLoadingBar.complete();
            }, function (error) {
                if (error.status === 401) {
                    notificationService.displayError('Authenticate is required.');
                }
                else if (failure != null) {
                    failure(error);
                }
                cfpLoadingBar.complete();
            });
        }

        function get(url, params, success, failure) {
            cfpLoadingBar.start();
            authenticationService.setHeader();
            $http.get(url, params).then(function (result) {
                success(result);
                cfpLoadingBar.complete();
            }, function (error) {
                if (error.status === 401) {
                    notificationService.displayError('Authenticate is required.');
                }
                else if (failure != null) {
                    failure(error);
                }
                cfpLoadingBar.complete();
            });
        }
    }
})(angular.module('cvmk.common.module'));
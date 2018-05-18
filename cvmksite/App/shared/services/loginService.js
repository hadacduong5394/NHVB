(function (app) {
    'use strict';
    app.service('loginService',['$http', '$q', 'authenticationService', 'authData',
    function ($http, $q, authenticationService, authData) {
        var userInfo;
        var deferred;

        this.login = function (userName, password) {
            deferred = $q.defer();
            var data = "grant_type=password&username=" + userName + "&password=" + password;
            $http.post('/oauth/token', data, {
                headers:
                   { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).then(function (response) {
                userInfo = {
                    accessToken: response.data.access_token,
                    userName: userName
                };
                authenticationService.setTokenInfo(userInfo);
                authenticationService.init();
                deferred.resolve(null);
            }, function (err) {
                deferred.resolve(err);
            });
            return deferred.promise;
        }
    }]);
})(angular.module('cvmk.common.module'));
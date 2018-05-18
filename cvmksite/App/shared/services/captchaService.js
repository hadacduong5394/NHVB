(function (app) {
    'use strict';
    app.service('captchaService', ['$http', '$q', 'captChaData',
    function ($http, $q, captChaData) {
        var deferred;
        this.captcha = function () {
            deferred = $q.defer();
            $http.get("api/captcha/getcaptcha").then(function (result) {
                captChaData.data = "data:image/png;base64," + result.data.ImageByteArray;
                captChaData.hash = result.data.Hash;
                deferred.resolve(captChaData);
            }, function (error) {
                deferred.resolve(null);
            });
            return deferred.promise;
        }

        this.validate = function (captchaValue) {
            deferred = $q.defer();
            var config = {
                params: {
                    hash: captChaData.hash,
                    captchaValue: captchaValue
                }
            }
            $http.get("api/captcha/validatecaptcha", config).then(function (result) {
                deferred.resolve(result.data);
            }, function (error) {
                deferred.resolve(null);
            });
            return deferred.promise;
        }
    }]);
})(angular.module('cvmk.common.module'));
(function (app) {
    'use strict';
    app.service('accountService', accountService);
    accountService.$inject = ['$http', '$q'];
    function accountService($http, $q) {
        var deferred;
        this.changepassword = function (userName, currentPassword, newPassword) {
            deferred = $q.defer();
            var vm = {
                UserName: userName,
                CurrenPassword: currentPassword,
                NewPassWord: newPassword
            }

            $http.post('api/account/changepassword', vm).then(function (result) {
                deferred.resolve(null);
            }, function (error) {
                deferred.resolve(error.data);
            });

            return deferred.promise;
        }
    }
})(angular.module('cvmk.common.module'));
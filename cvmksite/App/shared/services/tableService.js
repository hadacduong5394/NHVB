(function (app) {
    'use strict';
    app.service('tableService', ['$http', '$q',
    function ($http, $q) {
        var deferred;
        this.isWorking = function (tableId) {
            deferred = $q.defer();
            var config = {
                params: {
                    tableId: tableId
                }
            }
            $http.get('api/room/getstatusworkingtable', config).then(function (result) {
                var isTrueSet = (result.data == 'true');
                deferred.resolve(isTrueSet);
            }, function (reason) {
                deferred.resolve(null);
            });
            return deferred.promise;
        }

        this.loadFloor = function () {
            deferred = $q.defer();
            $http.get('api/floor/getall', null).then(function (result) {
                deferred.resolve(result);
            }, function (reason) {
                deferred.resolve(null);
            });
            return deferred.promise;
        }

        this.loadtablenotworking = function () {
            deferred = $q.defer();
            $http.get('api/room/loadtablenotworking', null).then(function (result) {
                deferred.resolve(result);
            }, function (reason) {
                deferred.resolve(null);
            });
            return deferred.promise;
        }

        this.gencode = function () {
            deferred = $q.defer();
            $http.get('api/orders/genarecode', null).then(function (result) {
                deferred.resolve(result.data.code);
            }, function (reason) {
                deferred.resolve(null);
            });
            return deferred.promise;
        }
    }]);
})(angular.module('cvmk.common.module'));
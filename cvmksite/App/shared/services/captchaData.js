(function (app) {
    'use strict';
    app.factory('captChaData', [function () {
        var captcha = {
            data: null,
            hash: null
        }

        return captcha;
    }]);
})(angular.module('cvmk.common.module'));
(function (app) {
    app.controller('lostpasswordController', ['$scope', 'captchaService', '$injector', 'notificationService', 'apiService',
        function ($scope, captchaService, $injector, notificationService, apiService) {
            $scope.userName = '';
            $scope.email = '';
            $scope.dataCaptchaBase64 = null;
            $scope.loadCaptcha = function () {
                captchaService.captcha().then(function (response) {
                    if (response != null) {
                        $scope.dataCaptchaBase64 = response.data;
                    }
                });
            }
            $scope.loadCaptcha();

            $scope.send = function () {
                captchaService.validate($scope.captcha).then(function (result)
                {
                    debugger;
                    var info = {
                        UserName: $scope.userName,
                        Email: $scope.email
                    }
                    apiService.post('api/account/lostpassword', info, function (result) {
                        var stateService = $injector.get('$state');
                        notificationService.displaySuccess('Hãy sử dụng email vừa nhập để xác nhận thông tin.');
                        var state = $injector.get('$state');
                        state.go('login');
                    }, function (error) {
                        $scope.loadCaptcha();
                        $scope.captcha = "";
                        notificationService.displayError("Gửi thất bại.");
                    });
                });
            }
        }]);
})(angular.module('cvmk.app'));
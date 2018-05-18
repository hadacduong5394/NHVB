(function (app) {
    app.controller('loginController', ['$scope', 'loginService', 'captchaService', '$injector', 'notificationService',
        function ($scope, loginService, captchaService, $injector, notificationService) {
            $scope.loginData = {
                userName: "",
                password: ""
            };
            $scope.captcha = "";
            $scope.loginSubmit = function () {
                captchaService.validate($scope.captcha).then(function (result)
                {
                    if(result != null && result == "true"){
                        loginService.login($scope.loginData.userName, $scope.loginData.password).then(function (response) {
                            if (response != null && response.data.error != undefined) {
                                $scope.loadCaptcha();
                                $scope.captcha = "";
                                notificationService.displayError("Đăng nhập thất bại.");
                            }
                            else {
                                var stateService = $injector.get('$state');
                                stateService.go('worktable');
                            }
                        });
                    } else {
                        $scope.loadCaptcha();
                        $scope.captcha = "";
                        notificationService.displayError('Nhập sai mã xác thực');
                    }
                });
            }

            $scope.dataCaptchaBase64 = null;
            $scope.loadCaptcha = function () {
                captchaService.captcha().then(function (response) {
                    if (response != null) {
                        $scope.dataCaptchaBase64 = response.data;
                    }
                });
            }
            $scope.loadCaptcha();
        }]);
})(angular.module('cvmk.app'));
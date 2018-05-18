(function (app) {
    app.controller('topMenuController', topMenuController);
    topMenuController.$inject = ['$scope', 'apiService', 'notificationService', 'loginService', '$state', 'authenticationService', 'authData', 'accountService'];
    function topMenuController($scope, apiService, notificationService, loginService, $state, authenticationService, authData, accountService) {
        $scope.topmenu = [];
        $scope.loadTopMenu = function () {
            apiService.get('api/topmenu/gettopmenu', null, function (result) {
                $scope.topmenu = result.data
            }, function (error) {
                notificationService.displayError(error.statusText);
            });
        }
        $scope.loadTopMenu();

        $scope.logout = function () {
            apiService.post("api/account/logout", null, function () {
                authenticationService.removeToken();
                authData.authenticationData.IsAuthenticated = false;
                authData.authenticationData.userName = "";
                authData.authenticationData.accessToken = '';
                $state.go('login');
            }, null);
        }
        $scope.openmodalchangpassword = function () {
            $('#modalChangePassword').modal('show');
        }

        $scope.userdata = {
            currentpass: "",
            newpass: "",
            confirmpass: ""
        }
        $scope.changePassword = function () {
            if (!$scope.confirmFlag) {
                accountService.changepassword(authData.authenticationData.userName, $scope.userdata.currentpass, $scope.userdata.newpass).then(function (result) {
                    if (result == null || result == '') {
                        $('#modalChangePassword').modal('hide');
                        $scope.logout();
                        notificationService.displaySuccess("Đổi mật khẩu thành công.");
                    } else {
                        notificationService.displayError(result);
                    }
                });
            }
        }

        $scope.confirmFlag = false;
        $scope.confirmPass = function () {
            if ($scope.userdata.newpass != $scope.userdata.confirmpass) {
                $scope.confirmFlag = true;
            } else {
                $scope.confirmFlag = false;
            }
        }

        $scope.showChange = function () {
            $('#modalChangeInfo').modal('show');
        }

        $scope.comId = null;
        $scope.username = authData.authenticationData.userName;
        $scope.loadCompanyInfo = function () {
            var config = {
                params: {
                    username: authData.authenticationData.userName
                }
            }
            apiService.get('api/users/getbyusername', config, function (result) {
                $scope.comId = result.data.ComId;
            }, function (reason) {

            });
        }
        $scope.loadCompanyInfo();
    }
})(angular.module('cvmk.app'));
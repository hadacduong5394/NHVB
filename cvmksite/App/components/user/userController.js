(function (app) {
    app.controller('userController', userController);
    userController.$inject = ['$scope', '$state', 'apiService', 'notificationService', '$stateParams', '$ngBootbox', 'authData'];
    function userController($scope, $state, apiService, notificationService, $stateParams, $ngBootbox, authData) {
        $scope.page = 0;
        $scope.pageSize = 10;
        $scope.pagesCount = 0;
        $scope.totalCount = 0;
        $scope.users = [];
        $scope.username = "";
        $scope.email = "";
        $scope.phonenumber = "";
        $scope.teamid = 1;
        $scope.teams = [];
        $scope.pressEnter = function ($event) {
            if ($event.keyCode === 13) {
                $scope.getbyfilter();
            }
        }
        $scope.getbyfilter = function (page) {
            var config = {
                params: {
                    teamId: $scope.teamid,
                    userName: $scope.username,
                    email: $scope.email,
                    phoneNumber: $scope.phonenumber,
                    page: $scope.page,
                    pageSize: $scope.pageSize
                }
            }
            apiService.get('api/users/getbyfilter', config, function (result) {
                $scope.page = result.data.Page;
                $scope.pageSize = result.data.PageSize;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.users = result.data.Items;
                
            }, function (error) {
                notificationService.displayError(error.statusText);
            });
        }
        $scope.getbyfilter();

        $scope.loadCompanyInfo = function () {
            var config = {
                params: {
                    username: authData.authenticationData.userName
                }
            }
            apiService.get('api/users/getbyusername', config, function (result) {
                if(result.data.TeamId == 0){
                    $scope.teams = [{ "Id": 0, "Name": "Root" }, { "Id": 1, "Name": "Nhóm quản trị" }];
                } else {
                    $scope.teams = [{ "Id": 1, "Name": "Nhóm quản trị" }, { "Id": 2, "Name": "Nhóm nhân viên" }];
                }
            }, function (reason) {
                notificationService.displayError(error.statusText);
            });
        }
        $scope.loadCompanyInfo();

        $scope.delete = function (username) {
            var config = {
                params: {
                    userName: username
                }
            }
            $ngBootbox.confirm('Bạn có chắc chắn muốn xóa?').then(function () {
                apiService.del('api/users/delete', config, function (result) {
                    notificationService.displaySuccess(result.data);
                    $scope.getbyfilter();
                }, function (error) {
                    notificationService.displayError(error.statusText)
                });
            });
        }
    }
})(angular.module('user.module'));
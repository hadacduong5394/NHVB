(function (app) {
    app.controller('createuserController', createuserController);
    createuserController.$inject = ['$scope', '$state', 'apiService', 'notificationService', '$stateParams', '$ngBootbox', 'authData'];
    function createuserController($scope, $state, apiService, notificationService, $stateParams, $ngBootbox, authData) {
        $scope.groups = [];
        $scope.loadGroups = function () {
            apiService.get('api/groups/getgroups', null, function (result) {
                $scope.groups = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        $scope.loadGroups();
        $scope.createnew = function () {
            var config = {
                params: {
                    username: authData.authenticationData.userName
                }
            }
            apiService.get('api/users/getbyusername', config, function (result) {
                $scope.user.ComId = result.data.ComId;
                if(result.data.ComId == -1){
                    $scope.isRoot = true;
                }
                if (result.data.TeamId == 0) {
                    $scope.teams = [{ "Id": 0, "Name": "Root" }, { "Id": 1, "Name": "Nhóm quản trị" }];
                    $scope.user.TeamId = 1;
                } else {
                    $scope.teams = [{ "Id": 1, "Name": "Nhóm quản trị" }, { "Id": 2, "Name": "Nhóm nhân viên" }];
                }
            }, function (reason) {
                notificationService.displayError(error.statusText);
            });
        }
        $scope.createnew();

        $scope.companies = [];
        $scope.loadCompanys = function () {
            apiService.get('api/company/getall', null, function (result) {
                $scope.companies = result.data;
            }, function (reason) {
                notificationService.displayError(error.statusText);
            });
        }
        $scope.loadCompanys();

        $scope.isRoot = false;
        $scope.user = {
            TeamId: 2,
            UserName: "",
            PassWord: "",
            FullName: "",
            PhoneNumber: null,
            Email: "",
            Address: "",
            BirthDay: new Date(),
            Status: true,
            InputGroupId: "",
            ComId: -1
        }

        $scope.teams = [];

        $scope.create = function () {
            var flag = false;
            var lst = [];
            angular.forEach($scope.groups, function (item, index) {
                if (item.IsCheck == true) {
                    flag = true;
                    lst.push(item.Id);
                }
            });

            if (flag == false) {
                notificationService.displayWarning('Chọn ít nhất một nhóm người dùng cho người dùng này.');
                return false;
            } else {
                $scope.user.InputGroupId = JSON.stringify(lst);
                apiService.post('api/users/create', $scope.user, function (result) {
                    $state.go('user');
                    notificationService.displaySuccess(result.data);
                }, function (error) {
                    notificationService.displayError(error.data);
                });
            }
        }
    }
})(angular.module('user.module'));
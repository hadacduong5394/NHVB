(function (app) {
    app.controller('edituserController', edituserController);
    edituserController.$inject = ['$scope', '$state', 'apiService', 'notificationService', '$stateParams', '$ngBootbox', '$q', 'authData'];
    function edituserController($scope, $state, apiService, notificationService, $stateParams, $ngBootbox, $q, authData) {
        $scope.groups = [];
        var deferred;
        $scope.loadGroups = function () {
            deferred = $q.defer();
            apiService.get('api/groups/getgroupstoupdateuser', null, function (result) {
                $scope.groups = result.data;
                deferred.resolve(result.data);
            }, function (error) {
                deferred.resolve(null);
            });
            return deferred.promise;
        }
        $scope.companies = [];
        $scope.loadCompanys = function () {
            apiService.get('api/company/getall', null, function (result) {
                $scope.companies = result.data;
            }, function (reason) { });
        }
        $scope.loadCompanys();
        $scope.createnew = function () {
            var config = {
                params: {
                    username: authData.authenticationData.userName
                }
            }
            apiService.get('api/users/getbyusername', config, function (result) {
                $scope.user.ComId = result.data.ComId;
                if (result.data.ComId == -1) {
                    $scope.isRoot = true;
                }
                if (result.data.TeamId == 0) {
                    $scope.teams = [{ "Id": 0, "Name": "Root" }, { "Id": 1, "Name": "Nhóm quản trị" }];
                } else {
                    $scope.teams = [{ "Id": 1, "Name": "Nhóm quản trị" }, { "Id": 2, "Name": "Nhóm nhân viên" }];
                }
            }, function (reason) {
                notificationService.displayError(error.statusText);
            });
        }
        $scope.createnew();

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

        $scope.update = function () {
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
                apiService.put('api/users/update', $scope.user, function (result) {
                    $state.go('user');
                    notificationService.displaySuccess(result.data);
                }, function (error) {
                    notificationService.displayError(error.data);
                });
            }
        }

        $scope.getbykey = function () {
            $scope.loadGroups().then(function (reponse) {
                if (reponse != null) {
                    var config = {
                        params: {
                            username: $stateParams.username
                        }
                    }
                    apiService.get("api/users/getbyusername", config, function (result) {
                        $scope.user = result.data;
                        //$scope.user.PhoneNumber = parseInt(result.data.PhoneNumber);
                        $scope.lstChecked = result.data.Groups;
                        angular.forEach($scope.groups, function (item, index) {
                            angular.forEach($scope.lstChecked, function (c, i) {
                                if (item.Id == c.Id) {
                                    item.IsCheck = true;
                                }
                            });
                        });
                    }, function (error) {
                        notificationService.displayError(error.data);
                    });
                } else {
                    notificationService.displayError('Lỗi hệ thống, vui lòng thử lại sau');
                }
            });
        }
        $scope.getbykey();
    }
})(angular.module('user.module'));
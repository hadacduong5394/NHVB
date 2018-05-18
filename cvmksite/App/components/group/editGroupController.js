(function (app) {
    app.controller('editGroupController', editGroupController);
    editGroupController.$inject = ['$scope', 'notificationService', 'apiService', '$state', '$ngBootbox', '$stateParams', '$q'];
    function editGroupController($scope, notificationService, apiService, $state, $ngBootbox, $stateParams, $q) {
        var deferred;
        $scope.roles = [];
        $scope.loadRoles = function () {
            deferred = $q.defer();
            apiService.get("api/roles/getall", null, function (result) {
                $scope.roles = result.data;
                deferred.resolve(true);
            }, function (error) {
                deferred.resolve(false);
            });
            return deferred.promise;
        }
        
        $scope.group = {
            Id: 0,
            Descreption: '',
            Name: '',
            Status: false,
            CreateBy: '',
            CreateDate: '',
            InputRoles: '',
        }
        $scope.lstChecked = [];
        $scope.getbykey = function () {
            $scope.loadRoles().then(function (reponse) {
                if (reponse) {
                    apiService.get('api/groups/getbykey/' + $stateParams.id, null, function (result) {
                        $scope.group = result.data;
                        $scope.lstChecked = result.data.Roles;
                        angular.forEach($scope.roles, function (item, index) {
                            angular.forEach($scope.lstChecked, function (c, i) {
                                if (item.Id == c.Id) {
                                    item.IsCheck = true;
                                }
                            });
                        });
                    }, function (error) {
                        notificationService.displayError(error.data);
                    })
                } else {
                    notificationService.displayError('Lỗi hệ thống, vui lòng thử lại sau.');
                }
            });
            
        }
        $scope.getbykey();

        $scope.update = function () {
            var lst = [];
            angular.forEach($scope.roles, function (item, index) {
                if (item.IsCheck == true) {
                    lst.push(item.Id);
                }
            });
            $scope.group.InputRoles = JSON.stringify(lst);
            apiService.put('api/groups/update', $scope.group, function (result) {
                notificationService.displaySuccess(result.data);
                $state.go('group');
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
    }
})(angular.module('group.module'));
(function (app) {
    app.controller('editRoleController', editRoleController);
    editRoleController.$inject = ['$scope', 'notificationService', 'apiService', '$state', '$ngBootbox', '$stateParams'];
    function editRoleController($scope, notificationService, apiService, $state, $ngBootbox, $stateParams) {
        $scope.role = {
            Id: 0,
            Name: '',
            Descreption: '',
            Status: false,
            CreateBy: '',
            CreateDate: ''
        }

        $scope.getbykey = function () {
            apiService.get('api/roles/getbykey/' + $stateParams.id, null, function (result) {
                $scope.role = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        $scope.getbykey();

        $scope.update = function () {
            apiService.put('api/roles/update', $scope.role, function (result) {
                notificationService.displaySuccess(result.data);
                $state.go('role');
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
    }
})(angular.module('role.module'));
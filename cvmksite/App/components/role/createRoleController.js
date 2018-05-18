(function (app) {
    app.controller('createRoleController', createRoleController);
    createRoleController.$inject = ['$scope', 'notificationService', 'apiService', '$state', '$ngBootbox', '$http'];
    function createRoleController($scope, notificationService, apiService, $state, $ngBootbox, $http) {
        $scope.role = {
            Id: 0,
            Name: '',
            Descreption: '',
            Status: false
        }

        $scope.create = function () {
            apiService.post('api/roles/create', $scope.role, function (result) {
                notificationService.displaySuccess(result.data);
                $state.go('role');
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
    }
})(angular.module('role.module'));
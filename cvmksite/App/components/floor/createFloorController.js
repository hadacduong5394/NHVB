(function (app) {
    app.controller('createFloorController', createFloorController);
    createFloorController.$inject = ['$scope', 'notificationService', '$ngBootbox', '$state', 'apiService'];
    function createFloorController($scope, notificationService, $ngBootbox, $state, apiService) {
        $scope.entity = {
            Id: 0,
            Name: "",
            Descreption: "",
            Status: true,
            VIP: false
        }

        $scope.create = function () {
            apiService.post('api/floor/create', $scope.entity, function (result) {
                notificationService.displaySuccess(result.data);
                $state.go('floor');
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
    }
})(angular.module('floor.module'));
(function (app) {
    app.controller('createRoomController', createRoomController);
    createRoomController.$inject = ['$scope', 'notificationService', '$ngBootbox', '$state', 'apiService'];
    function createRoomController($scope, notificationService, $ngBootbox, $state, apiService) {
        $scope.floors = [];
        $scope.loadFloors = function () {
            apiService.get('api/floor/getall', null, function (result) {
                $scope.floors = result.data;
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
        $scope.loadFloors();

        $scope.entity = {
            Id: 0,
            FloorId: null,
            Name: "",
            Descreption: "",
            Status: true,
            IsWorking: false,
        }

        $scope.create = function () {
            apiService.post('api/room/create', $scope.entity, function (result) {
                notificationService.displaySuccess(result.data);
                $state.go('room');
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
    }
})(angular.module('room.module'));
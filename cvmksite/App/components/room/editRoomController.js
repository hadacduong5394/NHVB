(function (app) {
    app.controller('editRoomController', editRoomController);
    editRoomController.$inject = ['$scope', 'notificationService', '$ngBootbox', '$state', 'apiService', '$stateParams'];
    function editRoomController($scope, notificationService, $ngBootbox, $state, apiService, $stateParams) {
        $scope.entity = {
            Id: 0,
            FloorId: null,
            Name: "",
            Descreption: "",
            Status: true,
            IsWorking: false,
        }
        $scope.floors = [];
        $scope.loadFloors = function () {
            apiService.get('api/floor/getall', null, function (result) {
                $scope.floors = result.data;
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
        $scope.loadFloors();

        $scope.getbykey = function () {
            var config = {
                params: {
                    id: $stateParams.id
                }
            }
            apiService.get('api/room/getbykey', config, function (result) {
                $scope.entity = result.data;
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
        $scope.getbykey();

        $scope.update = function () {
            apiService.put('api/room/update', $scope.entity, function (result) {
                notificationService.displaySuccess(result.data);
                $state.go('room');
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
    }
})(angular.module('room.module'));
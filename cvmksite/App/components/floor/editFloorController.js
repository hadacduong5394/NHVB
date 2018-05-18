(function (app) {
    app.controller('editFloorController', editFloorController);
    editFloorController.$inject = ['$scope', 'notificationService', '$ngBootbox', '$state', 'apiService', '$stateParams'];
    function editFloorController($scope, notificationService, $ngBootbox, $state, apiService, $stateParams) {
        $scope.entity = {
            Id: 0,
            Name: "",
            Descreption: "",
            Status: true,
            VIP: false
        }

        $scope.getbykey = function () {
            var config = {
                params: {
                    id: $stateParams.id
                }
            }
            apiService.get('api/floor/getbykey', config, function (result) {
                $scope.entity = result.data;
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
        $scope.getbykey();

        $scope.update = function () {
            apiService.put('api/floor/update', $scope.entity, function (result) {
                notificationService.displaySuccess(result.data);
                $state.go('floor');
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
    }
})(angular.module('floor.module'));
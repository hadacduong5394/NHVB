(function (app) {
    app.controller('editTypeproductController', editTypeproductController);
    editTypeproductController.$inject = ['$scope', 'notificationService', 'apiService', '$state', '$ngBootbox', '$stateParams'];
    function editTypeproductController($scope, notificationService, apiService, $state, $ngBootbox, $stateParams) {
        $scope.entity = {
            Id: 0,
            Name: '',
            Status: false,
        }

        $scope.getbykey = function () {
            var config = {
                params: {
                    id: $stateParams.id
                }
            }
            apiService.get('api/typeproduct/getbykey', config, function (result) {
                $scope.entity = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        $scope.getbykey();

        $scope.update = function () {
            apiService.put('api/typeproduct/update', $scope.entity, function (result) {
                notificationService.displaySuccess('Cập nhật thành công.');
                $state.go('typeproduct');
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
    }
})(angular.module('typeproduct.module'));
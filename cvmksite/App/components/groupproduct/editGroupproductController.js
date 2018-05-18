(function (app) {
    app.controller('editGroupproductController', editGroupproductController);
    editGroupproductController.$inject = ['$scope', 'notificationService', 'apiService', '$state', '$ngBootbox', '$stateParams'];
    function editGroupproductController($scope, notificationService, apiService, $state, $ngBootbox, $stateParams) {
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
            apiService.get('api/groupproduct/getbykey', config, function (result) {
                $scope.entity = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        $scope.getbykey();

        $scope.update = function () {
            apiService.put('api/groupproduct/update', $scope.entity, function (result) {
                notificationService.displaySuccess('Cập nhật thành công.');
                $state.go('groupproduct');
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
    }
})(angular.module('groupproduct.module'));
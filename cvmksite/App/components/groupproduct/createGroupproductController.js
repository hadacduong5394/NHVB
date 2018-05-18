(function (app) {
    app.controller('createGroupproductController', createGroupproductController);
    createGroupproductController.$inject = ['$scope', 'notificationService', 'apiService', '$state'];
    function createGroupproductController($scope, notificationService, apiService, $state) {
        $scope.entity = {
            Id: 0,
            Name: '',
            Status: false
        }

        $scope.create = function () {
            apiService.post('api/groupproduct/create', $scope.entity, function (result) {
                notificationService.displaySuccess('Thêm mới thành công.');
                $state.go('groupproduct');
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
    }
})(angular.module('groupproduct.module'));
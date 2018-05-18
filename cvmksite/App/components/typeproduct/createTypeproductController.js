(function (app) {
    app.controller('createTypeproductController', createTypeproductController);
    createTypeproductController.$inject = ['$scope', 'notificationService', 'apiService', '$state'];
    function createTypeproductController($scope, notificationService, apiService, $state) {
        $scope.entity = {
            Id: 0,
            Name: '',
            Status: false
        }

        $scope.create = function () {
            apiService.post('api/typeproduct/create', $scope.entity, function (result) {
                notificationService.displaySuccess('Thêm mới thành công.');
                $state.go('typeproduct');
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
    }
})(angular.module('typeproduct.module'));
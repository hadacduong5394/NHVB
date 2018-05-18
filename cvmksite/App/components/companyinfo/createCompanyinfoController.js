(function (app) {
    app.controller('createCompanyinfoController', createCompanyinfoController);
    createCompanyinfoController.$inject = ['$scope', 'notificationService', 'apiService', '$state'];
    function createCompanyinfoController($scope, notificationService, apiService, $state) {
        $scope.entity = {
            Id: 0,
            Name: '',
            LongTittle: '',
            Status: false
        }

        $scope.create = function () {
            apiService.post('api/company/create', $scope.entity, function (result) {
                notificationService.displaySuccess('Thêm mới thành công.');
                $state.go('companyinfo');
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
    }
})(angular.module('companyinfo.module'));
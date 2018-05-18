(function (app) {
    app.controller('editCompanyinfoController', editCompanyinfoController);
    editCompanyinfoController.$inject = ['$scope', 'notificationService', 'apiService', '$state', '$ngBootbox', '$stateParams'];
    function editCompanyinfoController($scope, notificationService, apiService, $state, $ngBootbox, $stateParams) {
        $scope.entity = {
            Id: 0,
            Name: '',
            LongTittle: '',
            Status: false
        }

        $scope.getbykey = function () {
            var config = {
                params: {
                    id: $stateParams.id
                }
            }
            apiService.get('api/company/getbykey', config, function (result) {
                $scope.entity = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        $scope.getbykey();

        $scope.update = function () {
            apiService.put('api/company/changeinfo', $scope.entity, function (result) {
                notificationService.displaySuccess('Cập nhật thành công.');
                $state.go('companyinfo');
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
    }
})(angular.module('companyinfo.module'));
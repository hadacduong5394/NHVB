(function (app) {
    app.controller('companyInfoController', companyInfoController);
    companyInfoController.$inject = ['$scope', 'apiService', 'notificationService'];
    function companyInfoController($scope, apiService, notificationService) {
        $scope.company = {
            Id: 0,
            ImageIcon: '',
            LongName: '',
            ShortName: '',
            SrefLong: '',
            SrefShort: '',
            LongTittle: '',
            Name: ''
        }

        $scope.loadCompanyInfo = function () {
            apiService.get('api/company/info', null, function (result) {
                $scope.company = result.data;
            }, function (error) {
                notificationService.displayError(error.statusText);
            });
        }
        $scope.loadCompanyInfo();

        $scope.changeInfo = function () {
            apiService.put('api/company/changeinfo', $scope.company, function (result) {
                notificationService.displaySuccess(result.data);
                $('#modalChangeInfo').modal('hide');
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
    }
})(angular.module('cvmk.app'));
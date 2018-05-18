(function (app) {
    app.controller('createSupplierController', createSupplierController);
    createSupplierController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$state'];
    function createSupplierController($scope, apiService, notificationService, $ngBootbox, $state) {
        $scope.entity = {
            Id: 0,
            Code: "",
            Name: "",
            Address: "",
            Email: "",
            Logo: "",
            Descreption: "",
            TaxCode: "",
            PhoneNumber: "",
            Status: true
        }

        function gencode() {
            apiService.get('api/supplier/genarecode', null, function (result) {
                $scope.entity.Code = result.data.code;
            }, function (reason) {
                notificationService.displayError(reason.data.mess);
            });
        }
        gencode();


        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.entity.Logo = fileUrl;
                });
            }
            finder.popup();
        }
        $scope.create = function () {
            apiService.post('api/supplier/create', $scope.entity, function (result) {
                notificationService.displaySuccess(result.data);
                $state.go('supplier');
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
    }
})(angular.module('supplier.module'));
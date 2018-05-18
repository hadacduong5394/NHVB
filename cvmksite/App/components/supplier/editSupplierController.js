(function (app) {
    app.controller('editSupplierController', editSupplierController);
    editSupplierController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$state', '$stateParams'];
    function editSupplierController($scope, apiService, notificationService, $ngBootbox, $state, $stateParams) {
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
        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.entity.Logo = fileUrl;
                });
            }
            finder.popup();
        }
        $scope.getbykey = function () {
            var config = {
                params: {
                    id: $stateParams.id
                }
            }
            apiService.get('api/supplier/getbykey', config, function (result) {
                $scope.entity = result.data;
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
        $scope.getbykey();
        $scope.update = function () {
            apiService.put('api/supplier/update', $scope.entity, function (result) {
                notificationService.displaySuccess(result.data);
                $state.go('supplier');
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
    }
})(angular.module('supplier.module'));
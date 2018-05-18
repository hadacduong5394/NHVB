(function (app) {
    app.controller('editImportProductController', editImportProductController);
    editImportProductController.$inject = ['$scope', 'apiService', 'notificationService', '$state', '$ngBootbox', '$stateParams'];
    function editImportProductController($scope, apiService, notificationService, $state, $ngBootbox, $stateParams) {
        $scope.ip = {
            id: 0,
            Code: "",
            SuppierCode: null,
            Descreption: "",
            Status: true,
            TotalAmount: 0,
            JsonDetails: ""
        }
        $scope.suppliers = [];
        $scope.loadSuppliers = function () {
            apiService.get('api/supplier/getall', null, function (result) {
                $scope.suppliers = result.data;
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
        $scope.loadSuppliers();

        $scope.products = [];
        $scope.loadProduct = function () {
            apiService.get('api/material/getmaterialsforimport', null, function (result) {
                $scope.products = result.data;
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
        $scope.loadProduct();

        $scope.getbykey = function () {
            var config = {
                params: {
                    id: $stateParams.id
                }
            }
            apiService.get('api/importproduct/getbykey', config, function (result) {
                $scope.ip = result.data;
                $scope.lstDetail = JSON.parse(result.data.JsonDetails);
                i = $scope.lstDetail[0].Id;
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
        $scope.getbykey();

        var i = 0;
        $scope.lstDetail = [];

        $scope.newRow = function () {
            var detail = {
                Id: ++i,
                ProductId: null,
                Quantity: 1,
                Amount: 0,
                TotalAmount: 0,
                Descreption: ""
            }
            $scope.lstDetail.push(detail);
        }

        $scope.nextLine = function ($event) {
            if ($event.keyCode === 9) {
                $scope.newRow();
            }
        }

        $scope.removeRow = function (id) {
            total = 0;
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                angular.forEach($scope.lstDetail, function (item, index) {
                    if (item.Id == id) {
                        $scope.lstDetail.splice(index, 1);
                    }
                    total += item.TotalAmount;
                });
                $scope.ip.TotalAmount = total;
            });
        }

        $scope.reCalculate = function (id) {
            total = 0;
            angular.forEach($scope.lstDetail, function (item, index) {
                if (item.Id == id) {
                    item.TotalAmount = item.Quantity * item.Amount;
                }
                total += item.TotalAmount;
            });
            $scope.ip.TotalAmount = total;
        }

        $scope.update = function () {
            $scope.ip.JsonDetails = JSON.stringify($scope.lstDetail);
            apiService.put('api/importproduct/update', $scope.ip, function (result) {
                notificationService.displaySuccess(result.data);
                $state.go('importproduct');
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
    }
})(angular.module('importproduct.module'));
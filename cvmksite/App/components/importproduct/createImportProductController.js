(function (app) {
    app.controller('createImportProductController', createImportProductController);
    createImportProductController.$inject = ['$scope', 'apiService', 'notificationService', '$state', '$ngBootbox'];
    function createImportProductController($scope, apiService, notificationService, $state, $ngBootbox) {
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

        function gencode() {
            apiService.get('api/importproduct/genarecode', null, function (result) {
                $scope.ip.Code = result.data.code;
            }, function (reason) {
                notificationService.displayError(reason.data.mess);
            });
        }
        gencode();

        $scope.products = [];
        $scope.loadProduct = function () {
            apiService.get('api/material/getmaterialsforimport', null, function (result) {
                $scope.products = result.data;
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
        $scope.loadProduct();

        var i = 0;
        $scope.lstDetail = [{
            Id: 0,
            ProductId: null,
            Quantity: 1,
            Amount: 0,
            TotalAmount: 0,
            Descreption: ""
        }];

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

        $scope.create = function () {
            $scope.ip.JsonDetails = JSON.stringify($scope.lstDetail);
            apiService.post('api/importproduct/create', $scope.ip, function (result) {
                notificationService.displaySuccess(result.data);
                $state.go('importproduct');
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
    }
})(angular.module('importproduct.module'));
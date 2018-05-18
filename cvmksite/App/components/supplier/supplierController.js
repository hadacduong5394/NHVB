(function (app) {
    app.controller('supplierController', supplierController);
    supplierController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$state'];
    function supplierController($scope, apiService, notificationService, $ngBootbox, $state) {
        $scope.page = 0;
        $scope.pageSize = 10;
        $scope.pagesCount = 0;
        $scope.totalCount = 0;
        $scope.suppliers = [];
        $scope.name = "";
        $scope.email = "";
        $scope.taxcode = "";
        $scope.phone = "";

        $scope.pressEnter = function ($event) {
            if ($event.keyCode === 13) {
                $scope.getbyfilter();
            }
        }
        $scope.getbyfilter = function (page) {
            var config = {
                params: {
                    page: page | 0,
                    pageSize: $scope.pageSize,
                    name: $scope.name,
                    email: $scope.email,
                    taxcode: $scope.taxcode,
                    phone: $scope.phone,
                }
            }

            apiService.get('api/supplier/getbyfilter', config, function (result) {
                $scope.page = result.data.Page;
                $scope.pageSize = result.data.PageSize;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.suppliers = result.data.Items;
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
        $scope.getbyfilter();

        $scope.delete = function (id) {
            $ngBootbox.confirm('Bạn có chắc chắn muốn xóa?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('api/supplier/delete', config, function (result) {
                    notificationService.displaySuccess(result.data);
                    $scope.getbyfilter();
                }, function (reason) {
                    notificationService.displayError(reason.data);
                });
            });
        }
    }
})(angular.module('supplier.module'));
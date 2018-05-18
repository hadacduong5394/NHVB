(function (app) {
    app.controller('customerController', customerController);
    customerController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$state'];
    function customerController($scope, apiService, notificationService, $ngBootbox, $state) {
        $scope.page = 0;
        $scope.pageSize = 10;
        $scope.pagesCount = 0;
        $scope.totalCount = 0;
        $scope.customers = [];
        $scope.code = "";
        $scope.name = "";
        $scope.taxcode = "";
        $scope.email = "";
        $scope.pressEnter = function ($event) {
            if ($event.keyCode === 13) {
                $scope.getbyfilter();
            }
        }

        $scope.getbyfilter = function (page) {
            var config = {
                params: {
                    code: $scope.code,
                    name: $scope.name,
                    taxcode: $scope.taxcode,
                    email: $scope.email,
                    page: page | 0,
                    pageSize: $scope.pageSize,
                }
            }

            apiService.get('api/customers/getbyfilter', config, function (result) {
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.customers = result.data.Items;
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
                apiService.del('api/customers/delete', config, function (result) {
                    notificationService.displaySuccess(result.data);
                    $scope.getbyfilter();
                }, function (reason) {
                    notificationService.displayError(reason.data);
                });
            });
        }
    }
})(angular.module('customer.module'));
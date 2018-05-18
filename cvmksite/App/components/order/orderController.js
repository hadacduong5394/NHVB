(function (app) {
    app.controller('orderController', orderController);
    orderController.$inject = ['$scope', 'notificationService', 'apiService', '$state', '$ngBootbox'];
    function orderController($scope, notificationService, apiService, $state, $ngBootbox) {
        $scope.page = 0;
        $scope.pageSize = 10;
        $scope.pagesCount = 0;
        $scope.totalCount = 0;
        $scope.orders = [];
        $scope.orderCode = "";
        $scope.isPayment = true;
        $scope.start = new Date();
        $scope.end = new Date();

        $scope.getbyfilter = function (page) {
            var config = {
                params: {
                    orderCode: $scope.orderCode,
                    isPayment: $scope.isPayment,
                    start: $scope.start,
                    end: $scope.end,
                    page: page | 0,
                    pageSize: $scope.pageSize
                }
            }
            apiService.get('api/orders/getbyfilter', config, function (result) {
                $scope.page = result.data.Page;
                $scope.pageSize = result.data.PageSize;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.orders = result.data.Items;
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
        $scope.getbyfilter();

        $scope.order = {}
        $scope.orderdetails = [];
        $scope.viewDetail = function (id) {
            var config = {
                params: {
                    id: id
                }
            }
            apiService.get('api/orders/viewdetail', config, function (result) {
                $scope.order = result.data;
                $scope.orderdetails = JSON.parse(result.data.JsonOrderDetail);
                $('#modalViewDetail').modal('show');
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }

        $scope.orderInfo = {};
        $scope.moneyBack = 0;
        $scope.ispayment = false;
        $scope.payBackOrderInfo = function (id) {
            var config = {
                params: { orderId: id }
            }
            apiService.get('api/orders/getinfotopayback', config, function (result) {
                $scope.orderInfo = result.data;
                $('#modalPayback').modal('show');
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
        $scope.PayFull = function () {
            if ($scope.ispayment == true) {
                $scope.moneyBack = $scope.orderInfo.Total - $scope.orderInfo.Payed;
            } else {
                $scope.moneyBack = 0;
            }
        }
        $scope.payback = function () {
            var config = {
                params: {
                    orderId: $scope.orderInfo.Id,
                    money: $scope.moneyBack
                }
            }
            apiService.get('api/orders/payback', config, function (result) {
                $scope.orderInfo = {};
                $scope.moneyBack = 0;
                $scope.ispayment = false;
                notificationService.displaySuccess(result.data);
                $scope.getbyfilter();
                $('#modalPayback').modal('hide');
            }, function (reason) {
                $scope.orderInfo = {};
                $scope.moneyBack = 0;
                $scope.ispayment = false;
                notificationService.displayError(reason.statusText);
            });
        }
    }

    app.filter('paymentFilter', function () {
        return function (isPayment) {
            if (isPayment == true) {
                return "Đã thanh toán đủ";
            } else {
                return "Chưa thanh toán đủ";
            }
        }
    });
})(angular.module('order.module'));
(function (app) {
    app.controller('productController', productController);
    productController.$inject = ['$scope', 'apiService', 'notificationService', '$state', '$ngBootbox'];
    function productController($scope, apiService, notificationService, $state, $ngBootbox) {
        $scope.page = 0;
        $scope.pageSize = 10;
        $scope.pagesCount = 0;
        $scope.totalCount = 0;
        $scope.name = "";
        $scope.code = "";
        $scope.groupId = null;
        $scope.typeId = null;
        $scope.products = [];

        $scope.pressEnter = function ($event) {
            if ($event.keyCode === 13) {
                $scope.getbyfilter();
            }
        }

        $scope.getbyfilter = function (page) {
            var config = {
                params: {
                    name: $scope.name,
                    code: $scope.code,
                    page: page | 0,
                    pageSize: $scope.pageSize
                }
            }

            apiService.get('api/product/getbyfilter', config, function (result) {
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.products = result.data.Items;
            }, function (reason) {
                notificationSerivce.displayError(reason.statusText);
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

                apiService.del('api/product/delete', config, function (result) {
                    notificationService.displaySuccess(result.data);
                    $scope.getbyfilter();
                }, function (reason) {
                    notificationService.displayError(reason.data);
                });
            });
        }
        $scope.product = {}
        $scope.images = [];
        $scope.props = [];
        $scope.lstMaterialChoossed = [];
        $scope.viewDetail = function (id) {
            var config = {
                params: {
                    id: id
                }
            }
            apiService.get('api/product/getbykey', config, function (result) {
                $scope.product = result.data;
                $scope.images = JSON.parse(result.data.Images);
                $scope.props = JSON.parse(result.data.JsonProps);
                $scope.lstMaterialChoossed = JSON.parse($scope.product.JsonMaterials);
                $('#modalViewDetail').modal('show');
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
    }

    app.filter('rawHtml', ['$sce', function ($sce) {
        return function (val) {
            return $sce.trustAsHtml(val);
        };
    }]);
})(angular.module('product.module'));
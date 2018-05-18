(function (app) {
    app.controller('groupproductController', groupproductController);
    groupproductController.$inject = ['$scope', 'notificationService', 'apiService', '$state', '$ngBootbox'];
    function groupproductController($scope, notificationService, apiService, $state, $ngBootbox) {
        $scope.page = 0;
        $scope.pageSize = 10;
        $scope.pagesCount = 0;
        $scope.totalCount = 0;
        $scope.groupproducts = [];
        $scope.name = "";

        $scope.getbyfilter = function (page) {
            var config = {
                params: {
                    name: $scope.name,
                    page: page | 0,
                    pageSize: $scope.pageSize
                }
            }

            apiService.get('api/groupproduct/getbyfilter', config, function (result) {
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.groupproducts = result.data.Items;
            }, function (error) {
                notificationService.displayError(error.statusText);
            });
        }
        $scope.getbyfilter();
        $scope.pressEnter = function ($event) {
            if ($event.keyCode === 13) {
                $scope.getbyfilter();
            }
        }

        $scope.delete = function (id) {
            var config = {
                params: {
                    id: id
                }
            }
            $ngBootbox.confirm('Bạn có chắc chắn muốn xóa?').then(function () {
                apiService.del('api/groupproduct/delete', config, function (result) {
                    notificationService.displaySuccess(result.data);
                    $scope.getbyfilter();
                }, function (error) {
                    notificationService.displayError(error.statusText);
                });
            });
        }
    }
})(angular.module('groupproduct.module'));
(function (app) {
    app.controller('importProductController', importProductController);
    importProductController.$inject = ['$scope', 'apiService', 'notificationService', '$state', '$ngBootbox'];
    function importProductController($scope, apiService, notificationService, $state, $ngBootbox) {
        $scope.page = 0;
        $scope.pageSize = 10;
        $scope.pagesCount = 0;
        $scope.totalCount = 0;
        $scope.code = "";
        $scope.importproducts = [];

        $scope.pressEnter = function ($event) {
            if ($event.keyCode === 13) {
                $scope.getbyfilter();
            }
        }

        $scope.getbyfilter = function (page) {
            var config = {
                params: {
                    code: $scope.code,
                    page: page | 0,
                    pageSize: 10
                }
            }

            apiService.get('api/importproduct/getbyfilter', config, function (result) {
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.importproducts = result.data.Items;
            }, function (reason) {
                notificationService.displayError(reason.statusText);
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

                apiService.del('api/importproduct/delete', config, function (result) {
                    notificationService.displaySuccess(result.data);
                    $scope.getbyfilter();
                }, function (reason) {
                    notificationService.displayError(reason.data);
                });
            });
        }

        $scope.importinfo = {};
        $scope.details = [];
        $scope.viewDetail = function (id) {
            var config = {
                params: {
                    id: id
                }
            }

            apiService.get('api/importproduct/getbykey', config, function (result) {
                $scope.importinfo = result.data;
                $scope.details = JSON.parse(result.data.JsonDetails);
                $('#modalViewDetail').modal('show');
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
    }
})(angular.module('importproduct.module'));
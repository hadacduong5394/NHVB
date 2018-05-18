(function (app) {
    app.controller('topmenuController', topmenuController);
    topmenuController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$state'];
    function topmenuController($scope, apiService, notificationService, $ngBootbox, $state) {
        $scope.page = 0;
        $scope.pageSize = 10;
        $scope.pagesCount = 0;
        $scope.totalCount = 0;
        $scope.parentId = null;
        $scope.menus = [];

        $scope.parents = [];
        $scope.getparents = function () {
            apiService.get('api/topmenu/getparents', null, function (result) {
                $scope.parents = result.data;
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
        $scope.getparents();

        $scope.getbyfilter = function (page) {
            var config = {
                params: {
                    page: page | 0,
                    pageSize: $scope.pageSize,
                    parentId: $scope.parentId
                }
            }
            apiService.get('api/topmenu/getbyfilter', config, function (result) {
                $scope.page = result.data.Page;
                $scope.pageSize = result.data.PageSize;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.menus = result.data.Items;
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
                apiService.del('api/topmenu/delete', config, function (result) {
                    notificationService.displaySuccess(result.data);
                    $scope.getbyfilter();
                }, function (reason) {
                    notificationService.displayError(reason.data);
                });
            });
        }
    }
})(angular.module('topmenu.module'));
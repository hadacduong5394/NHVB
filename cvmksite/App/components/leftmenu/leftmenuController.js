(function (app) {
    app.controller('leftmenuController', leftmenuController);
    leftmenuController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$state'];
    function leftmenuController($scope, apiService, notificationService, $ngBootbox, $state) {
        $scope.page = 0;
        $scope.pageSize = 10;
        $scope.pagesCount = 0;
        $scope.totalCount = 0;
        $scope.parentId = null;
        $scope.menus = [];

        $scope.parents = [];

        $scope.getparents = function () {
            apiService.get('api/leftmenu/getparents', null, function (result) {
                $scope.parents = result.data;
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
        $scope.getparents();

        $scope.getbyfilter = function (page) {
            var config = {
                params: {
                    parentId: $scope.parentId,
                    page: page | 0,
                    pageSize: $scope.pageSize
                }
            }
            apiService.get('api/leftmenu/getbyfilter', config, function (result) {
                $scope.page = result.data.Page;
                $scope.pageSize = result.data.PageSize;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.menus = result.data.Items;
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
        $scope.getbyfilter();

        $scope.delete = function (id) {
            $ngBootbox.confirm('Bạn chắc chắn muốn xóa?').then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('api/leftmenu/delete', config, function (result) {
                    notificationService.displaySuccess(result.data);
                    $scope.getbyfilter();
                }, function (reason) {
                    notificationService.displayError(reason.data);
                });
            });
        }
    }
})(angular.module('leftmenu.module'));
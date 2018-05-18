(function (app) {
    app.controller('materialController', materialController);
    materialController.$inject = ['$scope', 'notificationService', 'apiService', '$state', '$ngBootbox'];
    function materialController($scope, notificationService, apiService, $state, $ngBootbox) {
        $scope.materials = [];
        $scope.page = 0;
        $scope.pageSize = 10;
        $scope.pagesCount = 0;
        $scope.totalCount = 0;
        $scope.code = "";
        $scope.name = "";

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
                    page: page | 0,
                    pageSize: $scope.pageSize,
                }
            }

            apiService.get('/api/material/getbyfilter', config, function (result) {
                $scope.page = result.data.Page;
                $scope.pageSize = result.data.PageSize;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.materials = result.data.Items;
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
        $scope.getbyfilter();

        $scope.delete = function (id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function(){
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('api/material/delete', config, function (result) {
                    notificationService.displaySuccess(result.data);
                    $scope.getbyfilter();
                }, function (reason) {
                    notificationService.displayError(reason.data);
                });
            });
        }
    }
})(angular.module("material.module"));
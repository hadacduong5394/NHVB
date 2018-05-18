(function (app) {
    app.controller('floorController', floorController);
    floorController.$inject = ['$scope', 'notificationService', '$ngBootbox', '$state', 'apiService'];
    function floorController($scope, notificationService, $ngBootbox, $state, apiService) {
        $scope.page = 0;
        $scope.pageSize = 10;
        $scope.pagesCount = 0;
        $scope.totalCount = 0;
        $scope.keyword = "";
        $scope.floors = [];

        $scope.pressEnter = function ($event) {
            if ($event.keyCode === 13) {
                $scope.getbyfilter();
            }
        }

        $scope.getbyfilter = function (page) {
            var config = {
                params: {
                    name: $scope.keyword,
                    page: page | 0,
                    pageSize: $scope.pageSize
                }
            }

            apiService.get('api/floor/getbyfilter', config, function (result) {
                $scope.floors = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
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
                apiService.del('api/floor/delete', config, function (result) {
                    notificationService.displaySuccess(result.data);
                    $scope.getbyfilter();
                }, function (reason) {
                    notificationService.displayError(reason.data);
                });
            });
        }
    }
})(angular.module('floor.module'));
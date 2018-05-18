(function (app) {
    app.controller('roomController', roomController);
    roomController.$inject = ['$scope', 'notificationService', '$ngBootbox', '$state', 'apiService'];
    function roomController($scope, notificationService, $ngBootbox, $state, apiService) {
        $scope.page = 0;
        $scope.pageSize = 10;
        $scope.pagesCount = 0;
        $scope.totalCount = 0;
        $scope.keyword = "";
        $scope.rooms = [];
        $scope.floorId = null;
        $scope.floors = [];
        $scope.loadFloor = function () {
            apiService.get('api/floor/getall', null, function (result) {
                $scope.floors = result.data;
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
        $scope.loadFloor();

        $scope.pressEnter = function ($event) {
            if ($event.keyCode === 13) {
                $scope.getbyfilter();
            }
        }

        $scope.getbyfilter = function (page) {
            var floorId = $scope.floorId;
            if ($scope.floorId == null) {
                floorId = -1;
            }
            var config = {
                params: {
                    floorId: floorId,
                    name: $scope.keyword,
                    page: page | 0,
                    pageSize: $scope.pageSize,
                }
            }

            apiService.get('api/room/getbyfilter', config, function (result) {
                $scope.rooms = result.data.Items;
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
                apiService.del('api/room/delete', config, function (result) {
                    notificationService.displaySuccess(result.data);
                    $scope.getbyfilter();
                }, function (reason) {
                    notificationService.displayError(reason.data);
                });
            });
        }
    }
    app.filter('isworkingFilter', function () {
        return function (status) {
            if (status == true) {
                return "Đang phục vụ";
            } else {
                return "Đang rảnh";
            }
        }
    });
})(angular.module('room.module'));
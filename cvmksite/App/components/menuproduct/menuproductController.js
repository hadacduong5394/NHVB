(function (app) {
    app.controller('menuproductController', menuproductController);
    menuproductController.$inject = ['$scope', 'notificationService', 'apiService', '$state', '$ngBootbox', 'tableService'];
    function menuproductController($scope, notificationService, apiService, $state, $ngBootbox, tableService) {
        $scope.menuproducts = [];
        $scope.page = 0;
        $scope.pageSize = 16;
        $scope.pagesCount = 0;
        $scope.totalCount = 0;
        $scope.groupId = null;
        $scope.typeId = null;
        $scope.floorId = null;
        $scope.code_name = "";
        $scope.groups = [];
        $scope.types = [];
        $scope.floors = [];

        $scope.loadTypes = function () {
            apiService.get('api/typeproduct/gettypeproducts', null, function (result) {
                $scope.types = result.data;
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
        $scope.loadGroups = function () {
            apiService.get('api/groupproduct/getgroupproducts', null, function (result) {
                $scope.groups = result.data;
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
        $scope.loadFloor = function () {
            tableService.loadFloor().then(function (result) {
                if (result != null) {
                    $scope.floors = result.data;
                    $scope.floorId = result.data[0].Id;
                    $scope.getbyfilter();
                } else {
                    notificationService.displayError(reason.statusText);
                }
            });
        }
        $scope.loadTypes();
        $scope.loadGroups();
        $scope.loadFloor();

        $scope.pressEnter = function ($event) {
            if ($event.keyCode === 13) {
                $scope.getbyfilter();
            }
        }
        $scope.getbyfilter = function (page) {
            if ($scope.groupId == null) {
                $scope.groupId = -1;
            }
            if ($scope.typeId == null) {
                $scope.typeId = -1;
            }
            var config = {
                params: {
                    groupId: $scope.groupId,
                    typeId: $scope.typeId,
                    page: page | 0,
                    pageSize: $scope.pageSize,
                    code_name: $scope.code_name,
                    floorId: $scope.floorId
                }
            }

            apiService.get('api/menuproduct/getmenuproducts', config, function (result) {
                $scope.menuproducts = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }

        $scope.images = [];
        $scope.showImages = function (id) {
            angular.forEach($scope.menuproducts, function (item, index) {
                if (item.Id == id) {
                    $scope.images = JSON.parse(item.MoreImages);
                    $('#modalViewDetail').modal('show');
                }
            });
        }
    }
})(angular.module('menuproduct.module'));
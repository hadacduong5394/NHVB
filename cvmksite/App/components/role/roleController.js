(function (app) {
    app.controller('roleController', roleController);
    roleController.$inject = ['$scope', 'notificationService', 'apiService', '$state', '$ngBootbox'];
    function roleController($scope, notificationService, apiService, $state, $ngBootbox) {
        $scope.page = 0;
        $scope.pageSize = 10;
        $scope.pagesCount = 0;
        $scope.totalCount = 0;
        $scope.roles = [];
        $scope.rolename = "";

        $scope.getbyfilter = function (page) {
            var config = {
                params: {
                    roleName: $scope.rolename,
                    page: page | 0,
                    pageSize: $scope.pageSize
                }
            }

            apiService.get('api/roles/getbyfilter', config, function (result) {
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.roles = result.data.Items;
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
                apiService.del('api/roles/delete', config, function (result) {
                    notificationService.displaySuccess(result.data);
                    $scope.getbyfilter();
                }, function (error) {
                    notificationService.displayError(error.statusText);
                });
            });
        }

        $scope.roleDetail = {
            Id: 0,
            Name: '',
            Descreption: '',
            Status: false,
            CreateBy: '',
            CreateDate: '',
            UpdateBy: '',
            UpdateDate: ''
        }
        $scope.viewDetail = function (id) {
            var config = {
                params: {
                    id: id
                }
            }
            apiService.get('api/roles/detail', config, function (result) {
                $scope.roleDetail = result.data;
                $('#modalDetail').modal('show');
            }, function (error) {
                notificationService.displayError(error.statusText);
            });
        }
    }
})(angular.module('role.module'));
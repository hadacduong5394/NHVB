(function (app) {
    app.controller('groupController', groupController);
    groupController.$inject = ['$scope', 'notificationService', 'apiService', '$state', '$ngBootbox'];
    function groupController($scope, notificationService, apiService, $state, $ngBootbox) {
        $scope.page = 0;
        $scope.pageSize = 10;
        $scope.pagesCount = 0;
        $scope.totalCount = 0;
        $scope.groups = [];
        $scope.keyWord = "";

        $scope.getbyfilter = function (page) {
            var config = {
                params: {
                    keyWord: $scope.keyWord,
                    page: page | 0,
                    pageSize: $scope.pageSize
                }
            }

            apiService.get('api/groups/getbyfilter', config, function (result) {
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.groups = result.data.Items;
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
                apiService.del('api/groups/delete', config, function (result) {
                    notificationService.displaySuccess(result.data);
                    $scope.getbyfilter();
                }, function (error) {
                    notificationService.displayError(error.statusText);
                });
            });
        }

        $scope.group = {
            Id: 0,
            Descreption: '',
            Name: '',
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
            apiService.get('api/groups/viewdetail', config, function (result) {
                $scope.roles = '';
                $scope.group = result.data;
                angular.forEach(result.data.Roles, function (item, index) {
                    $scope.roles += item.Descreption + ', ';
                });
                $('#modalDetail').modal('show');
            }, function (error) {
                notificationService.displayError(error.statusText);
            });
        }
    }
})(angular.module('group.module'));
(function (app) {
    app.controller('edittopmenuController', edittopmenuController);
    edittopmenuController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$state', '$stateParams'];
    function edittopmenuController($scope, apiService, notificationService, $ngBootbox, $state, $stateParams) {
        $scope.entity = {
            Id: 0,
            Name: "",
            ICon: "",
            OrderNumber: 0,
            ParentId: null,
            UI_SREF: "",
            Status: true
        }
        $scope.getbykey = function () {
            var config = {
                params: {
                    id: $stateParams.id
                }
            }
            apiService.get('api/topmenu/getbykey', config, function (result) {
                $scope.entity = result.data;
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
        $scope.getbykey();

        $scope.update = function () {
            apiService.put('api/topmenu/update', $scope.entity, function (result) {
                notificationService.displaySuccess(result.data);
                $state.go('topmenu');
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
        $scope.parents = [];
        $scope.getparents = function () {
            apiService.get('api/topmenu/getparents', null, function (result) {
                $scope.parents = result.data;
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
        $scope.getparents();

        $scope.loadlastorder = function () {
            var config = {
                params: {
                    parentId: $scope.entity.ParentId
                }
            }
            apiService.get('api/leftmenu/lastorder', config, function (result) {
                $scope.entity.OrderNumber = result.data;
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
    }
})(angular.module('topmenu.module'));
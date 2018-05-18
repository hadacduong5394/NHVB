(function (app) {
    app.controller('createtopmenuController', createtopmenuController);
    createtopmenuController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$state'];
    function createtopmenuController($scope, apiService, notificationService, $ngBootbox, $state) {
        $scope.entity = {
            Id: 0,
            Name: "",
            ICon: "",
            OrderNumber: 0,
            ParentId: null,
            UI_SREF: "",
            Status: true
        }
        $scope.create = function () {
            apiService.post('api/topmenu/create', $scope.entity, function (result) {
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
                notificationService.displayError(reason.statusText);
            });
        }
        $scope.getparents();

        $scope.loadlastorder = function () {
            var config = {
                params: {
                    parentId: $scope.entity.ParentId
                }
            }
            apiService.get('api/topmenu/lastorder', config, function (result) {
                $scope.entity.OrderNumber = result.data;
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
        $scope.loadlastorder();
    }
})(angular.module('topmenu.module'));
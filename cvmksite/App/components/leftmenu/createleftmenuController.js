(function (app) {
    app.controller('createleftmenuController', createleftmenuController);
    createleftmenuController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$state', '$q'];
    function createleftmenuController($scope, apiService, notificationService, $ngBootbox, $state, $q) {
        $scope.entity = {
            Id: 0,
            Name: "",
            Icon: "",
            OrderNumber: 0,
            ParentId: null,
            UI_SREF: "",
            Status: true
        }
        $scope.create = function () {
            apiService.post('api/leftmenu/create', $scope.entity, function (result) {
                notificationService.displaySuccess(result.data);
                $state.go('leftmenu');
            }, function (reason)
            {
                notificationService.displayError(reason.data);
            });
        }
        $scope.parents = [];
        $scope.getparents = function () {
            apiService.get('api/leftmenu/getparents', null, function (result) {
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
            apiService.get('api/leftmenu/lastorder', config, function (result) {
                $scope.entity.OrderNumber = result.data;
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
        $scope.loadlastorder();
    }
})(angular.module('leftmenu.module'));
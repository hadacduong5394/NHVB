(function (app) {
    app.controller('editleftmenuController', editleftmenuController);
    editleftmenuController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$state', '$q', '$stateParams'];
    function editleftmenuController($scope, apiService, notificationService, $ngBootbox, $state, $q, $stateParams) {
        $scope.entity = {
            Id: 0,
            Name: "",
            Icon: "",
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
            apiService.get('api/leftmenu/getbykey', config, function (result) {
                $scope.entity = result.data;
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
        $scope.getbykey();

        $scope.update = function () {
            apiService.put('api/leftmenu/update', $scope.entity, function (result) {
                notificationService.displaySuccess(result.data);
                $state.go('leftmenu');
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
        $scope.parents = [];
        $scope.getparents = function () {
            apiService.get('api/leftmenu/getparents', null, function (result) {
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
})(angular.module('leftmenu.module'));
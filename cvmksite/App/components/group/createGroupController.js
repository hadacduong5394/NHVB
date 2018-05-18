(function (app) {
    app.controller('createGroupController', createGroupController);
    createGroupController.$inject = ['$scope', 'notificationService', 'apiService', '$state', '$ngBootbox'];
    function createGroupController($scope, notificationService, apiService, $state, $ngBootbox) {
        $scope.roles = [];
        $scope.loadRoles = function () {
            apiService.get("api/roles/getall", null, function (result) {
                $scope.roles = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        $scope.loadRoles();

        $scope.group = {
            Id: 0,
            Descreption: '',
            Name: '',
            Status: false,
            CreateBy: '',
            CreateDate: '',
            InputRoles: '',
        }

        $scope.create = function () {
            var lst = [];
            angular.forEach($scope.roles, function (item, index) {
                if(item.IsCheck == true){
                    lst.push(item.Id);
                }
            });
            $scope.group.InputRoles = JSON.stringify(lst);
            apiService.post('api/groups/create', $scope.group, function (result) {
                notificationService.displaySuccess(result.data);
                $state.go('group');
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
    }
})(angular.module('group.module'));
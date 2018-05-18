(function (app) {
    app.controller('leftMenuController', leftMenuController);
    leftMenuController.$inject = ['$scope', 'apiService', 'notificationService'];
    function leftMenuController($scope, apiService, notificationService) {
        $scope.leftmenu = [];
        $scope.loadLeftMenu = function () {
            apiService.get('api/leftmenu/getleftmenu', null, function (result) {
                $scope.leftmenu = result.data
            }, function (error) {
                notificationService.displayError(error.statusText);
            });
        }
        $scope.loadLeftMenu();
    }
})(angular.module('cvmk.app'));
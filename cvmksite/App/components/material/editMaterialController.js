(function (app) {
    app.controller('editMaterialController', editMaterialController);
    editMaterialController.$inject = ['$scope', 'notificationService', 'apiService', '$state', '$ngBootbox', '$stateParams'];
    function editMaterialController($scope, notificationService, apiService, $state, $ngBootbox, $stateParams) {
        $scope.material = {
            Id: 0,
            Code: "",
            Name: "",
            Descreption: "",
            Image: "",
            Quantity: 0,
            RootPrice: 0,
            Unit: "",
            ComId: 0,
            Status: true
        }

        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.material.Image = fileUrl;
                });
            }
            finder.popup();
        }

        $scope.getbykey = function () {
            var config = {
                params: {
                    id: $stateParams.id
                }
            }
            apiService.get('api/material/getbykey', config, function (result) {
                $scope.material = result.data;
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
        $scope.getbykey();

        $scope.update = function () {
            apiService.put('api/material/update', $scope.material, function (result) {
                notificationService.displaySuccess(result.data);
                $state.go('material');
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
    }
})(angular.module("material.module"));
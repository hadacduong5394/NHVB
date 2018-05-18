(function (app) {
    app.controller('createMaterialController', createMaterialController);
    createMaterialController.$inject = ['$scope', 'notificationService', 'apiService', '$state', '$ngBootbox'];
    function createMaterialController($scope, notificationService, apiService, $state, $ngBootbox) {
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

        function gencode() {
            apiService.get('api/material/genarecode', null, function (result) {
                $scope.material.Code = result.data.code;
            }, function (reason) {
                notificationService.displayError(reason.data.mess);
            });
        }
        gencode();

        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.material.Image = fileUrl;
                });
            }
            finder.popup();
        }

        $scope.create = function () {
            apiService.post('api/material/create', $scope.material, function (result) {
                notificationService.displaySuccess(result.data);
                $state.go('material');
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
    }
})(angular.module("material.module"));
(function (app) {
    app.controller('editCustomerController', editCustomerController);
    editCustomerController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$state', '$stateParams'];
    function editCustomerController($scope, apiService, notificationService, $ngBootbox, $state, $stateParams) {
        $scope.entity = {
            Id: 0,
            Type: '1',
            Name: "",
            Company: "",
            BirthDay: new Date(),
            Code: "",
            TaxCode: "",
            Avatar: "",
            PhoneNumber: "",
            Email: "",
            Address: "",
            Descreption: "",
            Status: false
        }
        $scope.type = "1";
        //setting for datepicker
        $scope.dateOptions = {
            formatYear: 'yy',
            maxDate: new Date(),
            startingDay: 1
        };
        $scope.datetimeFormat = ['M!/d!/yyyy'];
        $scope.openDate = function () {
            $scope.popupDate.opened = true;
        };
        $scope.popupDate = {
            opened: false
        };

        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.entity.Avatar = fileUrl;
            }
            finder.popup();
        }

        $scope.getbykey = function () {
            var config = {
                params: {
                    id: $stateParams.id
                }
            }
            apiService.get('api/customers/getbykey', config, function (result) {
                $scope.entity = result.data;
                $scope.entity.BirthDay = new Date(result.data.BirthDay);
                $scope.type = result.data.Type.toString();
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
        $scope.getbykey();

        $scope.update = function () {
            $scope.entity.Type = parseInt($scope.type);
            apiService.put('api/customers/update', $scope.entity, function (result) {
                notificationService.displaySuccess(result.data);
                $state.go('customer');
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
    }
})(angular.module('customer.module'));
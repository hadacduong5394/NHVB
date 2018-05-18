(function (app) {
    app.controller('createCustomerController', createCustomerController);
    createCustomerController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$state'];
    function createCustomerController($scope, apiService, notificationService, $ngBootbox, $state) {
        $scope.entity = {
            Id: 0,
            Type: 1,
            Name: "",
            Company: "",
            Code: "",
            BirthDay: new Date(),
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
        $scope.TypeOwn = {
            id: 1
        }
        $scope.TypeCompany = {
            id: 2
        }

        function gencode() {
            apiService.get('api/customers/genarecode', null, function (result) {
                $scope.entity.Code = result.data.code;
            }, function (reason) {
                notificationService.displayError(reason.data.mess);
            });
        }
        gencode();

        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.entity.Avatar = fileUrl
                });
            }
            finder.popup();
        }

        $scope.create = function () {
            $scope.entity.Type = parseInt($scope.type);
            apiService.post('api/customers/create', $scope.entity, function (result) {
                notificationService.displaySuccess(result.data);
                $state.go('customer');
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }
    }
})(angular.module('customer.module'));
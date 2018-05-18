(function (app) {
    app.controller('createProductController', createProductController);
    createProductController.$inject = ['$scope', 'apiService', 'notificationService', '$state', '$ngBootbox'];
    function createProductController($scope, apiService, notificationService, $state, $ngBootbox) {
        $scope.types = [];
        $scope.groups = [];
        $scope.loadTypes = function () {
            apiService.get('api/typeproduct/gettypeproducts', null, function (result) {
                $scope.types = result.data;
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
        $scope.loadGroups = function () {
            apiService.get('api/groupproduct/getgroupproducts', null, function (result) {
                $scope.groups = result.data;
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
        $scope.loadTypes();
        $scope.loadGroups();
        $scope.ckeditorOptions = {
            languague: 'vi',
            height: '200px'
        }

        function gencode() {
            apiService.get('api/product/genarecode', null, function (result) {
                $scope.product.BarCode = result.data.code;
            }, function (reason) {
                notificationService.displayError(reason.data.mess);
            });
        }
        gencode();

        $scope.product = {
            Id: 0,
            BarCode: "",
            Name: "",
            Images: "",
            Descreption: "",
            Content: "",
            Quantity: 0,
            TypeId: null,
            GroupId: null,
            Status: true,
            VIP: false,
            Unit: "",
            JsonProps: "",
            JsonMaterials: "",
            RootPrice: 0,
            Price: 0,
            Image: ""
        }
        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.product.Image = fileUrl;
                });
            }
            finder.popup();
        }
        $scope.Images = [];
        $scope.ChooseImages = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.Images.push(fileUrl);
                });
            }
            finder.popup();
        }

        $scope.create = function () {
            $scope.product.Images = JSON.stringify($scope.Images);
            $scope.product.JsonProps = JSON.stringify($scope.props);
            $scope.product.JsonMaterials = JSON.stringify($scope.lstMaterialChoossed);
            apiService.post('api/product/create', $scope.product, function (result) {
                notificationService.displaySuccess(result.data);
                $state.go('product');
            }, function (reason) {
                notificationService.displayError(reason.data);
            });
        }

        $scope.showAddTypeProduct = function () {
            $('#modalAddType').modal('show');
        }
        $scope.type = {
            Id: 0,
            Name: "",
            Status: true
        }
        $scope.addTypeProduct = function () {
            if ($scope.type.Name != null && $scope.type.Name != '') {
                apiService.post('api/typeproduct/create', $scope.type, function (result) {
                    notificationService.displaySuccess("Thêm mới loại sản phẩm thành công.");
                    $('#modalAddType').modal('hide');
                    $scope.types = result.data;
                    $scope.type = {
                        Id: 0,
                        Name: "",
                        Status: true
                    }
                }, function (reason) {
                    notificationService.displayError(reason.data);
                });
            }
        }

        $scope.showAddGroupProduct = function () {
            $('#modalAddGroup').modal('show');
        }
        $scope.group = {
            Id: 0,
            Name: "",
            Status: true
        }
        $scope.addGropuProduct = function () {
            if ($scope.group.Name != null && $scope.group.Name != '') {
                apiService.post('api/groupproduct/create', $scope.group, function (result) {
                    notificationService.displaySuccess("Thêm mới nhóm sản phẩm thành công.");
                    $('#modalAddGroup').modal('hide');
                    $scope.groups = result.data;
                    $scope.group = {
                        Id: 0,
                        Name: "",
                        Status: true
                    }
                }, function (reason) {
                    notificationService.displayError(reason.data);
                });
            }
        }

        $scope.props = [];
        var i = 0;
        $scope.prop = {
            Id: 0,
            ProductId: 0,
            Key: "",
            Value: ""
        }
        $scope.errorProp = "";
        $scope.addProp = function () {
            if ($scope.prop.Key == '' || $scope.prop.Value == '') {
                $scope.errorProp = "Thuộc tính hoặc giá trị không được trống."
            } else {
                $scope.prop.Id = ++i;
                $scope.props.push($scope.prop);
                $scope.prop = {
                    ProductId: 0,
                    Key: "",
                    Value: ""
                }
                $scope.errorProp = "";
                $('#modalAddProps').modal('hide');
            }
        }
        $scope.showModalProp = function () {
            $('#modalAddProps').modal('show');
        }
        $scope.deleteProp = function (id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa thuộc tính này?').then(function () {
                angular.forEach($scope.props, function (item, index) {
                    if (item.Id == id) {
                        $scope.props.splice(index, 1);
                    }
                });
            });
        }

        $scope.page = 0;
        $scope.pageSize = 10;
        $scope.pagesCount = 0;
        $scope.totalCount = 0;
        $scope.keywordMaterial = "";
        $scope.materials = [];
        $scope.loadMaterials = function (page) {
            var config = {
                params: {
                    keyword: $scope.keywordMaterial,
                    page: page | 0,
                    pageSize: $scope.pageSize
                }
            }
            apiService.get('api/material/getmaterials', config, function (result) {
                $scope.page = result.data.Page;
                $scope.pageSize = result.data.PageSize;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.materials = result.data.Items;
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
        $scope.loadMaterials();

        $scope.lstMaterialChoossed = [];
        $scope.chosseMaterial = function (id, code, name, unit, price) {
            var confligStatus = false;
            var currentIndex = 0;
            angular.forEach($scope.lstMaterialChoossed, function (item, index) {
                if (item.MaterialId == id) {
                    confligStatus = true;
                    currentIndex = index;
                }
            });
            if (confligStatus == true) {
                $scope.lstMaterialChoossed[currentIndex].Quantity++;
                $scope.lstMaterialChoossed[currentIndex].Amount = $scope.lstMaterialChoossed[currentIndex].Price * $scope.lstMaterialChoossed[currentIndex].Quantity;
            } else {
                var material = {
                    MaterialId: id,
                    ProductId: 0,
                    MaterialCode: code,
                    MaterialName: name,
                    Quantity: 1,
                    Price: price,
                    Unit: unit,
                    Amount: price
                }
                $scope.lstMaterialChoossed.push(material);
            }

            ReCalulateRootPrice();
        }
        $scope.ChangeMaterial = function (id) {
            var totalAmount = 0;
            angular.forEach($scope.lstMaterialChoossed, function (item, index) {
                if (item.MaterialId == id) {
                    item.Amount = item.Price * item.Quantity;
                }
                totalAmount += item.Amount;
            });
            $scope.product.RootPrice = totalAmount;
        }
        $scope.deleteMaterialChoosed = function (id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?').then(function () {
                angular.forEach($scope.lstMaterialChoossed, function (item, index) {
                    if (item.MaterialId == id) {
                        $scope.lstMaterialChoossed.splice(index, 1);
                    }
                });
            });

            ReCalulateRootPrice();
        }

        function ReCalulateRootPrice() {
            var totalAmount = 0;
            angular.forEach($scope.lstMaterialChoossed, function (item, index) {
                totalAmount += item.Amount;
            });
            $scope.product.RootPrice = totalAmount;
        }
    }
})(angular.module('product.module'));
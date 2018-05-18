(function (app) {
    app.controller('worktableController', worktableController);
    worktableController.$inject = ['$scope', 'notificationService', 'apiService', '$state', '$ngBootbox', 'tableService'];
    function worktableController($scope, notificationService, apiService, $state, $ngBootbox, tableService) {
        $scope.worktables = [];
        $scope.floors = [];
        $scope.floorId = null;
        $scope.loadFloor = function () {
            tableService.loadFloor().then(function (result) {
                if(result != null){
                    $scope.floors = result.data;
                    $scope.floorId = result.data[0].Id;
                    $scope.loadTables();
                    $scope.getbyfilter();
                } else {
                    notificationService.displayError(reason.statusText);
                }
            });
        }
        $scope.loadFloor();
        $scope.loadTables = function () {
            var config = {
                params: {
                    floorId: $scope.floorId
                }
            }
            apiService.get('api/room/getbyfloorid', config, function (result) {
                $scope.worktables = result.data;
                $scope.order = {
                    Id: 0,
                    Code: "",
                    Descreption: "",
                    TableId: 0,
                    TableName: "",
                    CustomerName: "Khách lẻ",
                    CustomerPhone: "",
                    CustomerEmail: "",
                    CustomerAddress: "",
                    EmployeeId: 0,
                    EmployeeName: "",
                    Sale: 0,
                    Payed: 0,
                    TotalAmount: 0,
                    Total: 0,
                    Status: false,
                    JsonOrderDetail: ""
                }
                $scope.getbyfilter();
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
        
        $scope.choossingTable = { Id: 0, Name: "" };
        $scope.choosseTable = function (tableId, tableName) {
            tableService.isWorking(tableId).then(function (result) {
                if (result != null && result == true) {
                    var config = {
                        params: {
                            tableId: tableId
                        }
                    }
                    apiService.get('api/orders/gettableproduct', config, function (result) {
                        $scope.order = result.data;
                        $scope.orderdetails = JSON.parse(result.data.JsonOrderDetail);
                        $scope.choossingTable = { Id: tableId, Name: tableName };
                    }, function (reason) {
                        notificationService.displayError(reason.data);
                    });
                } else {
                    $scope.order = {
                        Id: 0,
                        Code: "",
                        Descreption: "",
                        TableId: 0,
                        TableName: "",
                        CustomerName: "Khách lẻ",
                        CustomerPhone: "",
                        CustomerEmail: "",
                        CustomerAddress: "",
                        EmployeeId: 0,
                        EmployeeName: "",
                        Sale: 0,
                        Payed: 0,
                        TotalAmount: 0,
                        Total: 0,
                        Status: false,
                        JsonOrderDetail: ""
                    }
                    $scope.orderdetails = [];
                    $scope.choossedProducts = [];
                    $scope.choossingTable = { Id: tableId, Name: tableName };
                    $('#modalMenuProduct').modal('show');
                }
            });
        }

        $scope.groups = [];
        $scope.types = [];
        $scope.menuproducts = [];
        $scope.page = 0;
        $scope.pageSize = 6;
        $scope.pagesCount = 0;
        $scope.totalCount = 0;
        $scope.groupId = null;
        $scope.typeId = null;
        $scope.code_name = "";
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
        $scope.pressEnter = function ($event) {
            if ($event.keyCode === 13) {
                $scope.getbyfilter();
            }
        }
        $scope.getbyfilter = function (page) {
            var config = {
                params: {
                    groupId: $scope.groupId,
                    typeId: $scope.typeId,
                    page: page | 0,
                    pageSize: $scope.pageSize,
                    code_name: $scope.code_name,
                    floorId: $scope.floorId
                }
            }

            apiService.get('api/menuproduct/getmenuproducts', config, function (result) {
                $scope.menuproducts = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }

        function isChoossed(id) {
            var flag = false;
            angular.forEach($scope.choossedProducts, function (item, index) {
                if (item.ProductId == id) {
                    flag = true;
                }
            });
            return flag;
        }
        var orderdetailsIndex = 0;
        $scope.choossedProducts = [];
        $scope.choosseProduct = function (id) {
            var flag = isChoossed(id);
            if (flag) {
                angular.forEach($scope.choossedProducts, function (item, index) {
                    if (item.ProductId == id) {
                        item.Quantity++;
                    }
                    item.Amount = item.Price * item.Quantity;
                });
            } else {
                angular.forEach($scope.menuproducts, function (item, index) {
                    if (item.Id == id) {
                        var orderdetail = {
                            Id: ++orderdetailsIndex,
                            OrderId: 0,
                            ProductId: item.Id,
                            ProductName: item.Name,
                            ProductCode: item.BarCode,
                            Price: item.Price,
                            Quantity: 1,
                            Amount: item.Price * 1
                        }
                        $scope.choossedProducts.push(orderdetail);
                    }
                });
            }
        }
        $scope.deleteProductChoossed = function (id) {
            angular.forEach($scope.choossedProducts, function (item, index) {
                if (item.Id == id) {
                    $scope.choossedProducts.splice(index, 1);
                }
            });
        }

        $scope.order = {
            Id: 0,
            Code: "",
            Descreption: "",
            TableId: 0,
            TableName: "",
            CustomerName: "Khách lẻ",
            CustomerPhone: "",
            CustomerEmail: "",
            CustomerAddress: "",
            EmployeeId: 0,
            EmployeeName: "",
            Sale: 0,
            Payed: 0,
            TotalAmount: 0,
            Total: 0,
            Status: false,
            JsonOrderDetail: ""
        }
        $scope.orderdetails = [];
        $scope.Order = function (id) {
            if ($scope.choossingTable.Id != 0) {
                $scope.order.TableId = $scope.choossingTable.Id;
                $scope.order.TableName = $scope.choossingTable.Name;
                angular.forEach($scope.choossedProducts, function (item, index) {
                    $scope.order.TotalAmount += item.Amount;
                });
                $scope.order.JsonOrderDetail = JSON.stringify($scope.choossedProducts);
                tableService.gencode().then(function (result) {
                    debugger;
                    if (result != null) {
                        $scope.order.Code = result;
                        apiService.post('api/orders/orderproduct', $scope.order, function (result) {
                            angular.forEach($scope.worktables, function (item, index) {
                                if (item.Id == result.data.TableId) {
                                    item.IsWorking = result.data.TableStatus;
                                }
                            });
                            $scope.order = result.data;
                            $scope.orderdetails = JSON.parse(result.data.JsonOrderDetail);
                            $('#modalMenuProduct').modal('hide');
                        }, function (reason) {
                            notificationService.displayError(reason.data);
                        });
                    } else {
                        notificationService.displayError('lỗi hệ thống.');
                    }
                });
                
            }
        }
        $scope.PayFull = function () {
            $scope.order.Payed = $scope.order.TotalAmount - $scope.order.Sale;
            $scope.order.Status = true;
            $scope.order.Total = $scope.order.TotalAmount - $scope.order.Sale;
        }
        $scope.ReCal = function () {
            $scope.order.Total = $scope.order.TotalAmount - $scope.order.Sale;
            if ($scope.order.Status) {
                $scope.order.Payed = $scope.order.TotalAmount - $scope.order.Sale;
            }
        }
        $scope.changeQuantityOrderDetail = function (id) {
            var dt = {
                OrderId: $scope.order.Id,
                DetailId: id,
                Quantity: 0
            }
            angular.forEach($scope.orderdetails, function (item, index) {
                if (item.Id == id) {
                    dt.Quantity = item.Quantity;
                    item.Amount = item.Quantity * item.Price;
                }
            });
            apiService.put('api/orders/changequantityproductinorder', dt, function (result) {
                $scope.order = result.data;
            }, function (reason) {
                notificationService.displayError(reason.data.toString());
            });
        }

        $scope.addProductOrder = function (id) {
            $('#modalAddProductOrder').modal('show');
        }
        $scope.addProductToDetails = function (productId) {
            var entity = {}
            angular.forEach($scope.menuproducts, function (item, index) {
                if (item.Id == productId) {
                    entity = {
                        Id: 0,
                        OrderId: $scope.order.Id,
                        ProductId: item.Id,
                        ProductName: item.Name,
                        ProductCode: item.BarCode,
                        Quantity: 1,
                        Price: item.Price,
                        Amount: 1 * item.Price,
                    }
                }
            });
            apiService.post('api/orders/addproducttoorderdetail', entity, function (result) {
                $scope.order = result.data;
                $scope.orderdetails = JSON.parse(result.data.JsonOrderDetail);
                $('#modalAddProductOrder').modal('hide');
            }, function (reason) {
                notificationService.displayError(reason.data.toString());
            });
        }
        $scope.deleteDetailOfOrder = function (id) {
            $ngBootbox.confirm('Bạn có chắc chắn muốn xóa?').then(function () {
                var config = {
                    params: {
                        orderId: $scope.order.Id,
                        detailId: id
                    }
                }
                apiService.del('api/orders/deletedetailinorder', config, function (result) {
                    $scope.order = result.data;
                    $scope.orderdetails = JSON.parse(result.data.JsonOrderDetail);
                }, function (reason) {
                    notificationService.displayError(reason.data.toString());
                });
            });
        }
        $scope.CancleOrder = function (orderId) {
            $ngBootbox.confirm('Bạn chắc chắn muốn hủy đơn hàng này?').then(function () {
                var config = {
                    params: {
                        orderId: $scope.order.Id
                    }
                }

                apiService.del('api/orders/cancleorder', config, function (result) {
                    Reset();
                    notificationService.displaySuccess(result.data.toString());
                }, function (reason) {
                    notificationService.displayError(reason.data.toString());
                });
            });
        }
        $scope.Payment = function () {
            $ngBootbox.confirm('Thanh toán hóa đơn ' + $scope.order.Code + '?').then(function () {
                apiService.post('api/orders/payment', $scope.order, function (result) {
                    notificationService.displaySuccess(result.data);
                    Reset();
                }, function (reason) {
                    notificationService.displayError(reason.data);
                });
            });
        }

        $scope.tablesnotworking = [];
        $scope.loadTableNotWorking = function () {
            var config = {
                params: {
                    floorId: $scope.floorId
                }
            }
            apiService.get('api/room/loadtablenotworking', config, function (result) {
                $scope.tablesnotworking = result.data;
                $('#modalTableNotWorking').modal('show');
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }

        $scope.tablesworking = [];
        $scope.loadTableWorking = function () {
            var config = {
                params: {
                    momentTableId: $scope.choossingTable.Id,
                    floorId: $scope.floorId
                }
            }
            apiService.get('api/room/loadtableworking', config, function (result) {
                $scope.tablesworking = result.data;
                $('#modalTableWorking').modal('show');
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }

        $scope.changeTable = function (id, tablename) {
            $ngBootbox.confirm('Bạn chắc chắn muốn chuyển hóa đơn từ bàn ' + $scope.choossingTable.Name + ' tới bản ' + tablename + '?').then(function () {
                var config = {
                    params: {
                        orderId: $scope.order.Id,
                        toTableId: id
                    }
                }
                apiService.get('api/orders/changetable', config, function (result) {
                    $scope.order = result.data;
                    $scope.orderdetails = JSON.parse(result.data.JsonOrderDetail);
                    $scope.choossingTable = { Id: result.data.TableId, Name: result.data.TableName };
                    $scope.loadTables();
                    $('#modalTableNotWorking').modal('hide');
                    notificationService.displaySuccess('Chuyển bàn thành công.');
                }, function (reason) {
                    notificationService.displayError(reason.data);
                });
            });
        }

        $scope.combineTable = function (toTableId, toTableName) {
            $ngBootbox.confirm('Bạn chắc chắn muốn gộp hóa đơn bàn ' + $scope.choossingTable.Name + ' với bản ' + toTableName + '?').then(function () {
                var config = {
                    params: {
                        orderId: $scope.order.Id,
                        toTableId: toTableId
                    }
                }
                apiService.get('api/orders/combinetable', config, function (result) {
                    $scope.order = result.data;
                    $scope.orderdetails = JSON.parse(result.data.JsonOrderDetail);
                    $scope.choossingTable = { Id: result.data.TableId, Name: result.data.TableName };
                    $scope.loadTables();
                    $('#modalTableWorking').modal('hide');
                    notificationService.displaySuccess('Chuyển bàn thành công.');
                }, function (reason) {
                    notificationService.displayError(reason.data);
                });
            });
        }

        function Reset() {
            $scope.choossingTable = { Id: 0, Name: "" };
            $scope.order = {
                Id: 0,
                Code: "",
                Descreption: "",
                TableId: 0,
                TableName: "",
                CustomerName: "Khách lẻ",
                CustomerPhone: "",
                CustomerEmail: "",
                CustomerAddress: "",
                EmployeeId: 0,
                EmployeeName: "",
                Sale: 0,
                Payed: 0,
                TotalAmount: 0,
                Total: 0,
                Status: false,
                JsonOrderDetail: ""
            }
            $scope.orderdetails = [];
            $scope.loadTables();
        }
    }
    app.filter('isWorkingTableFilter', function () {
        return function (IsWorking) {
            if (IsWorking == true) {
                return "Phục vụ";
            } else {
                return "Đang rảnh";
            }
        }
    });
})(angular.module('worktable.module'));
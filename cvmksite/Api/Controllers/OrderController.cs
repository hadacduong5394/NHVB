using cvmk.context.IdentityConfiguration;
using cvmk.service.Interface;
using cvmksite.Models;
using cvmksite.Models.ViewModel;
using hdcore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace cvmksite.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/orders")]
    public class OrderController : ShareApiController
    {
        [HttpPost]
        [Route("orderproduct")]
        public HttpResponseMessage OrderProduct(HttpRequestMessage request, OrderViewModel vm)
        {
            var orderSrv = IoC.Resolve<IOrderService>();
            var order = vm.UpdateModel();
            var lstDetail = OrderDetailViewModel.UpdateModels(vm.DeSerilizableDetail());
            string mess = "";
            int orderId = order.Id;
            if (orderSrv.OrderProduct(vm.TableId, order, lstDetail, out mess, out orderId))
            {
                var entity = orderSrv.GetbyKey(orderId);
                var orderVm = new OrderViewModel
                {
                    Id = entity.Id,
                    Code = entity.Code,
                    Descreption = entity.Descreption,
                    CreateDate = entity.CreateDate,
                    CustomerAddress = entity.CustomerAddress,
                    CustomerEmail = entity.CustomerEmail,
                    CustomerName = entity.CustomerName,
                    CustomerPhone = entity.CustomerPhone,
                    EmployeeId = entity.EmployeeId,
                    EmployeeName = entity.EmployeeName,
                    Payed = entity.Payed,
                    Sale = entity.Sale,
                    TableId = entity.TableId,
                    TableName = entity.TableName,
                    Status = entity.Status,
                    Total = entity.Total,
                    TotalAmount = entity.TotalAmount
                };
                var odDetail = IoC.Resolve<IOrderDetailService>()
                    .GetMulti(n => n.OrderId == orderId)
                    .Select(n => new OrderDetailViewModel {
                        Id = n.Id,
                        OrderId = n.OrderId,
                        ProductId = n.ProductId,
                        ProductCode = n.ProductCode,
                        ProductName = n.ProductName,
                        Quantity = n.Quantity,
                        Price = n.Price,
                        Amount = n.Amount
                    }).ToList();
                orderVm.SerilizableDetail(odDetail);
                return request.CreateResponse(HttpStatusCode.OK, orderVm);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [Route("gettableproduct")]
        [HttpGet]
        public HttpResponseMessage GetTableProduct(HttpRequestMessage request, int tableId)
        {
            try
            {
                var orderSrv = IoC.Resolve<IOrderService>();
                var order = orderSrv.GetCurrentOrder(tableId);
                var entity = orderSrv.GetbyKey(order.Id);
                var orderVm = new OrderViewModel
                {
                    Id = entity.Id,
                    Code = entity.Code,
                    CreateDate = entity.CreateDate,
                    CustomerAddress = entity.CustomerAddress,
                    CustomerEmail = entity.CustomerEmail,
                    CustomerName = entity.CustomerName,
                    CustomerPhone = entity.CustomerPhone,
                    EmployeeId = entity.EmployeeId,
                    EmployeeName = entity.EmployeeName,
                    Payed = entity.Payed,
                    Sale = entity.Sale,
                    TableId = entity.TableId,
                    TableName = entity.TableName,
                    Status = entity.Status,
                    Total = entity.Total,
                    TotalAmount = entity.TotalAmount
                };
                var odDetail = IoC.Resolve<IOrderDetailService>()
                    .GetMulti(n => n.OrderId == order.Id)
                    .Select(n => new OrderDetailViewModel
                    {
                        Id = n.Id,
                        OrderId = n.OrderId,
                        ProductId = n.ProductId,
                        ProductCode = n.ProductCode,
                        ProductName = n.ProductName,
                        Quantity = n.Quantity,
                        Price = n.Price,
                        Amount = n.Amount
                    }).ToList();
                orderVm.SerilizableDetail(odDetail);
                return request.CreateResponse(HttpStatusCode.OK, orderVm);
            }
            catch (Exception ex)
            {
                Log(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, "Lỗi hệ thống.");
            }
        }

        [Route("addproducttoorderdetail")]
        [HttpPost]
        public HttpResponseMessage AddProductToOrderDetail(HttpRequestMessage request, OrderDetailViewModel vm)
        {
            var mess = "";
            var detailSrv = IoC.Resolve<IOrderDetailService>();
            var orderSrv = IoC.Resolve<IOrderService>();
            var entity = vm.UpdateModel();
            if (orderSrv.AddProductToOrder(vm.OrderId, entity, out mess))
            {
                var orderEntity = orderSrv.GetbyKey(vm.OrderId);
                var orderVm = new OrderViewModel
                {
                    Id = orderEntity.Id,
                    Code = orderEntity.Code,
                    Descreption = orderEntity.Descreption,
                    CreateDate = orderEntity.CreateDate,
                    CustomerAddress = orderEntity.CustomerAddress,
                    CustomerEmail = orderEntity.CustomerEmail,
                    CustomerName = orderEntity.CustomerName,
                    CustomerPhone = orderEntity.CustomerPhone,
                    EmployeeId = orderEntity.EmployeeId,
                    EmployeeName = orderEntity.EmployeeName,
                    Payed = orderEntity.Payed,
                    Sale = orderEntity.Sale,
                    TableId = orderEntity.TableId,
                    TableName = orderEntity.TableName,
                    Status = orderEntity.Status,
                    Total = orderEntity.Total,
                    TotalAmount = orderEntity.TotalAmount
                };
                var odDetail = detailSrv.GetByOrderId(orderEntity.Id)
                    .Select(n => new OrderDetailViewModel
                    {
                        Id = n.Id,
                        OrderId = n.OrderId,
                        ProductId = n.ProductId,
                        ProductCode = n.ProductCode,
                        ProductName = n.ProductName,
                        Quantity = n.Quantity,
                        Price = n.Price,
                        Amount = n.Amount
                    }).ToList();
                orderVm.SerilizableDetail(odDetail);
                return request.CreateResponse(HttpStatusCode.OK, orderVm);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [HttpPut]
        [Route("changequantityproductinorder")]
        public HttpResponseMessage ChangeQuantityProductInOrder(HttpRequestMessage request, ChangeProductQuantityOrderModelDetail dt)
        {
            var mess = "";
            var detailSrv = IoC.Resolve<IOrderDetailService>();
            var orderSrv = IoC.Resolve<IOrderService>();
            if (orderSrv.ChangeQuantityProduct(dt.OrderId, dt.DetailId, dt.Quantity, out mess))
            {
                var orderEntity = orderSrv.GetbyKey(dt.OrderId);
                var orderVm = new OrderViewModel
                {
                    Id = orderEntity.Id,
                    Code = orderEntity.Code,
                    Descreption = orderEntity.Descreption,
                    CreateDate = orderEntity.CreateDate,
                    CustomerAddress = orderEntity.CustomerAddress,
                    CustomerEmail = orderEntity.CustomerEmail,
                    CustomerName = orderEntity.CustomerName,
                    CustomerPhone = orderEntity.CustomerPhone,
                    EmployeeId = orderEntity.EmployeeId,
                    EmployeeName = orderEntity.EmployeeName,
                    Payed = orderEntity.Payed,
                    Sale = orderEntity.Sale,
                    TableId = orderEntity.TableId,
                    TableName = orderEntity.TableName,
                    Status = orderEntity.Status,
                    Total = orderEntity.Total,
                    TotalAmount = orderEntity.TotalAmount
                };
                var odDetail = detailSrv.GetByOrderId(orderEntity.Id)
                    .Select(n => new OrderDetailViewModel
                    {
                        Id = n.Id,
                        OrderId = n.OrderId,
                        ProductId = n.ProductId,
                        ProductCode = n.ProductCode,
                        ProductName = n.ProductName,
                        Quantity = n.Quantity,
                        Price = n.Price,
                        Amount = n.Amount
                    }).ToList();
                orderVm.SerilizableDetail(odDetail);
                return request.CreateResponse(HttpStatusCode.OK, orderVm);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [HttpDelete]
        [Route("deletedetailinorder")]
        public HttpResponseMessage DeleteDetailOrder(HttpRequestMessage request, int orderId, int detailId)
        {
            var mess = "";
            var detailSrv = IoC.Resolve<IOrderDetailService>();
            var orderSrv = IoC.Resolve<IOrderService>();
            if (orderSrv.DeleteDetail(orderId, detailId, out mess))
            {
                var orderEntity = orderSrv.GetbyKey(orderId);
                var orderVm = new OrderViewModel
                {
                    Id = orderEntity.Id,
                    Code = orderEntity.Code,
                    CreateDate = orderEntity.CreateDate,
                    CustomerAddress = orderEntity.CustomerAddress,
                    CustomerEmail = orderEntity.CustomerEmail,
                    CustomerName = orderEntity.CustomerName,
                    CustomerPhone = orderEntity.CustomerPhone,
                    EmployeeId = orderEntity.EmployeeId,
                    EmployeeName = orderEntity.EmployeeName,
                    Payed = orderEntity.Payed,
                    Sale = orderEntity.Sale,
                    TableId = orderEntity.TableId,
                    TableName = orderEntity.TableName,
                    Status = orderEntity.Status,
                    Total = orderEntity.Total,
                    TotalAmount = orderEntity.TotalAmount
                };
                var odDetail = detailSrv.GetByOrderId(orderEntity.Id)
                    .Select(n => new OrderDetailViewModel
                    {
                        Id = n.Id,
                        OrderId = n.OrderId,
                        ProductId = n.ProductId,
                        ProductCode = n.ProductCode,
                        ProductName = n.ProductName,
                        Quantity = n.Quantity,
                        Price = n.Price,
                        Amount = n.Amount
                    }).ToList();
                orderVm.SerilizableDetail(odDetail);
                return request.CreateResponse(HttpStatusCode.OK, orderVm);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [HttpDelete]
        [Route("cancleorder")]
        public HttpResponseMessage CancleOrder(HttpRequestMessage request, int orderId)
        {
            var detailSrv = IoC.Resolve<IOrderDetailService>();
            var orderSrv = IoC.Resolve<IOrderService>();
            string mess = "";
            if (orderSrv.CancleOrder(orderId, out mess))
            {
                return request.CreateResponse(HttpStatusCode.OK, mess);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [HttpPost]
        [Route("payment")]
        public HttpResponseMessage Payment(HttpRequestMessage request, OrderViewModel vm)
        {
            string mess = "";
            if (IoC.Resolve<IOrderService>().Payment(vm.UpdateModel(), vm.TableId, out mess))
            {
                return request.CreateResponse(HttpStatusCode.OK, mess);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [HttpGet]
        [Route("getbyfilter")]
        public HttpResponseMessage GetbyFilter(HttpRequestMessage request, string orderCode, DateTime? start, DateTime? end, bool? isPayment, int page = 0, int pageSize = 10)
        {
            var orderSrv = IoC.Resolve<IOrderService>();
            int total = 0;
            int comId = CurrentUser.Instance.User.ComId;
            var items = orderSrv.GetbyFilter(comId, orderCode, start, end, isPayment, page, pageSize, out total).Select(n => new OrderViewModel {
                Id = n.Id,
                Code = n.Code,
                CreateDateString = n.CreateDate.ToString("dd/MM/yyyy"),
                CustomerName = n.CustomerName,
                EmployeeName = n.EmployeeName,
                Total = n.Total,
                IsPayment = n.IsPayment,
                Status = n.Status
            }).ToList();

            var result = new PaginationSet<OrderViewModel>
            {
                Page = page,
                Items = items,
                PageSize = pageSize,
                TotalCount = total
            };
            return request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("viewdetail")]
        public HttpResponseMessage ViewDetail(HttpRequestMessage request, int id)
        {
            var orderSrv = IoC.Resolve<IOrderService>();
            var floorSrv = IoC.Resolve<IFloorService>();
            var tableSrv = IoC.Resolve<IRoomService>();
            var order = orderSrv.GetbyKey(id);
            var table = tableSrv.GetbyKey(order.TableId);
            var ordervm = new OrderViewModel
            {
                Id = order.Id,
                Code = order.Code,
                Descreption = order.Descreption,
                CreateDate = order.CreateDate,
                CreateDateString = order.CreateDate.ToString("dd/MM/yyyy"),
                CustomerName = order.CustomerName,
                CustomerAddress = order.CustomerAddress,
                CustomerEmail = order.CustomerEmail,
                CustomerPhone = order.CustomerPhone,
                EmployeeName = order.EmployeeName,
                EmployeeId = order.EmployeeId,
                Payed = order.Payed,
                Sale = order.Sale,
                IsPayment = order.IsPayment,
                Status = order.Status,
                TableId = order.TableId,
                TableName = order.TableName + " - " + floorSrv.GetbyKey(table.FloorId).Name,
                TotalAmount = order.TotalAmount,
                Total = order.Total
            };
            var odDetail = IoC.Resolve<IOrderDetailService>()
                    .GetByOrderId(ordervm.Id)
                    .Select(n => new OrderDetailViewModel
                    {
                        Id = n.Id,
                        OrderId = n.OrderId,
                        ProductId = n.ProductId,
                        ProductCode = n.ProductCode,
                        ProductName = n.ProductName,
                        Quantity = n.Quantity,
                        Price = n.Price,
                        Amount = n.Amount
                    }).ToList();
            ordervm.SerilizableDetail(odDetail);
            return request.CreateResponse(HttpStatusCode.OK, ordervm);
        }

        [Route("changetable")]
        [HttpGet]
        public HttpResponseMessage ChangeTable(HttpRequestMessage request, int orderId, int toTableId)
        {
            var orderSrv = IoC.Resolve<IOrderService>();
            var detailSrv = IoC.Resolve<IOrderDetailService>();
            string mess = "";
            if (orderSrv.ChangeTable(orderId, toTableId, out mess))
            {
                var orderEntity = orderSrv.GetbyKey(orderId);
                var orderVm = new OrderViewModel
                {
                    Id = orderEntity.Id,
                    Code = orderEntity.Code,
                    Descreption = orderEntity.Descreption,
                    CreateDate = orderEntity.CreateDate,
                    CustomerAddress = orderEntity.CustomerAddress,
                    CustomerEmail = orderEntity.CustomerEmail,
                    CustomerName = orderEntity.CustomerName,
                    CustomerPhone = orderEntity.CustomerPhone,
                    EmployeeId = orderEntity.EmployeeId,
                    EmployeeName = orderEntity.EmployeeName,
                    Payed = orderEntity.Payed,
                    Sale = orderEntity.Sale,
                    TableId = orderEntity.TableId,
                    TableName = orderEntity.TableName,
                    Status = orderEntity.Status,
                    Total = orderEntity.Total,
                    TotalAmount = orderEntity.TotalAmount
                };
                var odDetail = detailSrv.GetByOrderId(orderEntity.Id)
                    .Select(n => new OrderDetailViewModel
                    {
                        Id = n.Id,
                        OrderId = n.OrderId,
                        ProductId = n.ProductId,
                        ProductCode = n.ProductCode,
                        ProductName = n.ProductName,
                        Quantity = n.Quantity,
                        Price = n.Price,
                        Amount = n.Amount
                    }).ToList();
                orderVm.SerilizableDetail(odDetail);
                return request.CreateResponse(HttpStatusCode.OK, orderVm);
            }

            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [Route("combinetable")]
        [HttpGet]
        public HttpResponseMessage CombineTable(HttpRequestMessage request, int orderId, int toTableId)
        {
            var orderSrv = IoC.Resolve<IOrderService>();
            var detailSrv = IoC.Resolve<IOrderDetailService>();
            string mess = "";
            int orderIdWorking = 0;
            if (orderSrv.CombineTable(orderId, toTableId, out orderIdWorking, out mess))
            {
                var orderEntity = orderSrv.GetbyKey(orderIdWorking);
                var orderVm = new OrderViewModel
                {
                    Id = orderEntity.Id,
                    Code = orderEntity.Code,
                    Descreption = orderEntity.Descreption,
                    CreateDate = orderEntity.CreateDate,
                    CustomerAddress = orderEntity.CustomerAddress,
                    CustomerEmail = orderEntity.CustomerEmail,
                    CustomerName = orderEntity.CustomerName,
                    CustomerPhone = orderEntity.CustomerPhone,
                    EmployeeId = orderEntity.EmployeeId,
                    EmployeeName = orderEntity.EmployeeName,
                    Payed = orderEntity.Payed,
                    Sale = orderEntity.Sale,
                    TableId = orderEntity.TableId,
                    TableName = orderEntity.TableName,
                    Status = orderEntity.Status,
                    Total = orderEntity.Total,
                    TotalAmount = orderEntity.TotalAmount
                };
                var odDetail = detailSrv.GetByOrderId(orderEntity.Id)
                    .Select(n => new OrderDetailViewModel
                    {
                        Id = n.Id,
                        OrderId = n.OrderId,
                        ProductId = n.ProductId,
                        ProductCode = n.ProductCode,
                        ProductName = n.ProductName,
                        Quantity = n.Quantity,
                        Price = n.Price,
                        Amount = n.Amount
                    }).ToList();
                orderVm.SerilizableDetail(odDetail);
                return request.CreateResponse(HttpStatusCode.OK, orderVm);
            }

            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [Route("getinfotopayback")]
        [HttpGet]
        public HttpResponseMessage GetInfoToPayBack(HttpRequestMessage request, int orderId)
        {
            var order = IoC.Resolve<IOrderService>().GetbyKey(orderId);
            var rs = new OrderViewModel
            {
                Id = order.Id,
                Code = order.Code,
                TableName = order.TableName,
                Total = order.Total,
                Payed = order.Payed,
                CustomerName = order.CustomerName,
                Descreption = order.Descreption
            };
            return request.CreateResponse(HttpStatusCode.OK, rs);
        }

        [HttpGet]
        [Route("payback")]
        public HttpResponseMessage PayBack(HttpRequestMessage request, int orderId, decimal money)
        {
            string mess = "";
            var orderSrv = IoC.Resolve<IOrderService>();
            if (orderSrv.PayBack(orderId, money, out mess))
            {
                return request.CreateResponse(HttpStatusCode.OK, mess);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [HttpGet]
        [Route("genarecode")]
        public HttpResponseMessage GenareCode(HttpRequestMessage request)
        {
            string rs = string.Empty;
            if (IoC.Resolve<IOrderCodeService>().GenCode(out rs))
            {
                return request.CreateResponse(HttpStatusCode.OK, new { code = rs });
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, new { mess = "lỗi hệ thống, vui lòng thử lại sau." });
        }
    }
}

using cvmk.context.domain;
using cvmk.context.IdentityConfiguration;
using cvmk.service.Interface;
using hdcore;
using hddata.DBFactory;
using hddata.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.service.Implement
{
    public class OrderService : BaseService<Order, int>, IOrderService
    {
        private readonly IErrorService log;
        public OrderService(IDbFactory dbFactory) : base(dbFactory)
        {
            log = IoC.Resolve<IErrorService>();
        }

        public bool OrderProduct(int tableId, Order order, IList<OrderDetail> orderdetails, out string message, out int orderId)
        {
            BeginTran();
            try
            {
                order.ComId = CurrentUser.Instance.User.ComId;
                order.CreateDate = DateTime.Now;
                CreateNew(order);
                CommitChange();
                var orderDetailSrv = IoC.Resolve<IOrderDetailService>();
                foreach (var item in orderdetails)
                {
                    item.OrderId = order.Id;
                    orderDetailSrv.CreateNew(item);
                }
                orderDetailSrv.CommitChange();

                var roomSrv = IoC.Resolve<IRoomService>();
                roomSrv.GetbyKey(tableId).IsWorking = true;
                roomSrv.CommitChange();

                CommitTran();
                message = "";
                orderId = order.Id;
                return true;
            }
            catch (Exception ex)
            {
                RollbackTran();
                log.TryLog(ex);
                message = hdcore.Utils.TextHelper.ERROR_SYSTEM;
                orderId = order.Id;
                return false;
            }
        }

        public bool Delete(int id, out string message)
        {
            throw new NotImplementedException();
        }

        public IList<Order> GetbyFilter(string code, int currentPage, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Order GetCurrentOrder(int tableId)
        {
            var startToDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            var endToDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            return Query.Where(n => n.TableId == tableId && n.CreateDate >= startToDay && n.CreateDate <= endToDay && n.IsPayment == false).OrderByDescending(n => n.Id).FirstOrDefault();
        }

        public bool AddProductToOrder(int orderId, OrderDetail orderdetail, out string message)
        {
            try
            {
                BeginTran();
                var dtSrv = IoC.Resolve<IOrderDetailService>();
                if (!dtSrv.Query.Any(n => n.OrderId == orderId && n.ProductId == orderdetail.ProductId))
                {
                    dtSrv.CreateNew(orderdetail);
                }
                else
                {
                    var orderdtentity = dtSrv.Query.FirstOrDefault(n => n.OrderId == orderId && n.ProductId == orderdetail.ProductId);
                    orderdtentity.Quantity++;
                    dtSrv.Update(orderdtentity);
                }
                CommitChange();

                var order = GetbyKey(orderId);
                decimal totalAmount = 0;
                foreach (var item in dtSrv.GetByOrderId(orderId))
                {
                    totalAmount += (item.Quantity * item.Price);
                }
                order.TotalAmount = totalAmount;
                order.Total = order.TotalAmount - order.Sale - order.Payed;
                Update(order);
                CommitChange();

                CommitTran();
                message = "";
                return true;
            }
            catch (Exception ex)
            {
                RollbackTran();
                log.TryLog(ex);
                message = "Lỗi hệ thống.";
                return false;
            }
        }

        public bool ChangeQuantityProduct(int orderId, int detailId, int quantity, out string message)
        {
            try
            {
                BeginTran();
                var dtSrv = IoC.Resolve<IOrderDetailService>();
                var dt = dtSrv.GetbyKey(detailId);
                dt.Quantity = quantity;
                dt.Amount = dt.Price * dt.Quantity;
                dtSrv.Update(dt);
                dtSrv.CommitChange();

                var order = GetbyKey(orderId);
                decimal totalAmount = 0;
                foreach (var item in dtSrv.GetByOrderId(orderId))
                {
                    totalAmount += (item.Quantity * item.Price);
                }
                order.TotalAmount = totalAmount;
                order.Total = order.TotalAmount - order.Sale - order.Payed;
                Update(order);
                CommitChange();

                CommitTran();
                message = "";
                return true;
            }
            catch (Exception ex)
            {
                RollbackTran();
                log.TryLog(ex);
                message = "Lỗi hệ thống.";
                return false;
            }
        }

        public bool DeleteDetail(int orderId, int detailId, out string message)
        {
            try
            {
                BeginTran();
                var dtSrv = IoC.Resolve<IOrderDetailService>();
                dtSrv.Delete(detailId);
                dtSrv.CommitChange();

                var order = GetbyKey(orderId);
                decimal totalAmount = 0;
                foreach (var item in dtSrv.GetByOrderId(orderId))
                {
                    totalAmount += (item.Quantity * item.Price);
                }
                order.TotalAmount = totalAmount;
                order.Total = order.TotalAmount - order.Sale - order.Payed;
                Update(order);
                CommitChange();

                CommitTran();
                message = "";
                return true;
            }
            catch (Exception ex)
            {
                RollbackTran();
                log.TryLog(ex);
                message = "Lỗi hệ thống.";
                return false;
            }
        }

        public bool CancleOrder(int orderId, out string message)
        {
            try
            {
                BeginTran();
                var dtSrv = IoC.Resolve<IOrderDetailService>();
                dtSrv.DeleteMulti(n => n.OrderId == orderId);
                dtSrv.CommitChange();

                var entity = GetbyKey(orderId);
                var tableId = entity.TableId;
                Delete(orderId);
                CommitChange();

                var roomSrv = IoC.Resolve<IRoomService>();
                var table = roomSrv.GetbyKey(tableId);
                table.IsWorking = false;
                roomSrv.Update(table);
                roomSrv.CommitChange();

                CommitTran();
                message = "Hủy đơn hàng " + entity.Code + " thành công.";
                return true;
            }
            catch (Exception ex)
            {
                RollbackTran();
                log.TryLog(ex);
                message = "Lỗi hệ thống.";
                return false;
            }
        }

        public bool Payment(Order order, int tableId, out string message)
        {
            try
            {
                BeginTran();
                order.CreateDate = DateTime.Now;
                if (order.Payed >= order.Total)
                {
                    order.IsPayment = true;
                }
                else
                {
                    order.IsPayment = false;
                }
                order.Status = true;
                order.ComId = CurrentUser.Instance.User.ComId;
                Update(order);
                CommitChange();

                var roomSrv = IoC.Resolve<IRoomService>();
                var table = roomSrv.GetbyKey(tableId);
                table.IsWorking = false;
                roomSrv.Update(table);
                CommitChange();

                var dtSrv = IoC.Resolve<IOrderDetailService>();
                var mtSrv = IoC.Resolve<IMaterialService>();
                var proMtSrv = IoC.Resolve<IProductMaterialService>();
                var details = dtSrv.GetByOrderId(order.Id);
                foreach (var detail in details)
                {
                    var proMaterials = proMtSrv.GetbyProductId(detail.ProductId);
                    foreach (var item in proMaterials)
                    {
                        var material = mtSrv.GetbyKey(item.MaterialId);
                        material.Quantity -= item.Quantity * detail.Quantity;
                        mtSrv.Update(material);
                        mtSrv.CommitChange();
                    }
                }

                CommitTran();
                message = "Thanh toán thành công hóa đơn " + order.Code;
                return true;
            }
            catch (Exception ex)
            {
                RollbackTran();
                message = "Lỗi hệ thống";
                return false;
            }
        }

        public IList<Order> GetbyFilter(int comId, string code, DateTime? start, DateTime? end, bool? isPayment, int currentPage, int pageSize, out int total)
        {
            var query = Query.Where(n => n.ComId == comId);
            if (!string.IsNullOrEmpty(code))
            {
                query = query.Where(n => n.Code.Contains(code));
            }
            if (start.HasValue)
            {
                var startDay = new DateTime(start.Value.Year, start.Value.Month, start.Value.Day, 0, 0, 0);
                query = query.Where(n => n.CreateDate >= startDay);
            }
            if (end.HasValue)
            {
                var endDay = new DateTime(end.Value.Year, end.Value.Month, end.Value.Day, 23, 59, 59);
                query = query.Where(n => n.CreateDate <= endDay);
            }
            if (isPayment.HasValue)
            {
                query = query.Where(n => n.IsPayment == isPayment.Value);
            }
            query = query.OrderByDescending(n => n.Id);
            total = query.Count();
            return query.Skip(currentPage * pageSize).Take(pageSize).ToList();
        }

        public bool ChangeTable(int orderId, int toTableId, out string mess)
        {
            try
            {
                var tableSrv = IoC.Resolve<IRoomService>();
                var totable = tableSrv.GetbyKey(toTableId);
                if (totable.IsWorking)
                {
                    mess = "Bàn " + totable.Name + " đang phục vụ, hãy thử với bàn khác đang rảnh.";
                    return false;
                }
                BeginTran();
                var order = GetbyKey(orderId);

                var table = tableSrv.GetbyKey(order.TableId);
                table.IsWorking = false;
                tableSrv.Update(table);
                tableSrv.CommitChange();

                order.TableId = toTableId;
                order.TableName = totable.Name;
                Update(order);
                CommitChange();

                totable.IsWorking = true;
                tableSrv.Update(totable);
                tableSrv.CommitChange();

                CommitTran();
                mess = "Chuyển bàn thành công.";
                return true;
            }
            catch (Exception ex)
            {
                RollbackTran();
                log.TryLog(ex);
                mess = "Lỗi hệ thống.";
                return false;
            }
        }

        public bool CombineTable(int fromOrderId, int toTableId, out int workingOrderId, out string mess)
        {
            try
            {
                BeginTran();
                var orderFrom = GetbyKey(fromOrderId);
                var orderTo = GetCurrentOrder(toTableId);
                var orderdtSrv = IoC.Resolve<IOrderDetailService>();
                var tableSrv = IoC.Resolve<IRoomService>();

                var orderdetailsFrom = orderdtSrv.GetMulti(n => n.OrderId == orderFrom.Id);
                foreach (var dt in orderdetailsFrom)
                {
                    dt.OrderId = orderTo.Id;
                    orderdtSrv.Update(dt);
                }
                orderdtSrv.CommitChange();

                var table = tableSrv.GetbyKey(orderFrom.TableId);
                table.IsWorking = false;
                tableSrv.Update(table);
                tableSrv.CommitChange();

                Delete(fromOrderId);
                CommitChange();

                decimal totalAmount = 0;
                foreach (var item in orderdtSrv.GetByOrderId(orderTo.Id))
                {
                    totalAmount += (item.Quantity * item.Price);
                }
                orderTo.TotalAmount = totalAmount;
                orderTo.Total = orderTo.TotalAmount - orderTo.Sale - orderTo.Payed;
                Update(orderTo);
                CommitChange();

                CommitTran();
                mess = "Gộp bàn thành công.";
                workingOrderId = orderTo.Id;
                return true;
            }
            catch (Exception ex)
            {
                RollbackTran();
                log.TryLog(ex);
                mess = "Lỗi hệ thống.";
                workingOrderId = 0;
                return false;
            }
        }

        public bool PayBack(int orderId, decimal money, out string mess)
        {
            try
            {
                var order = GetbyKey(orderId);
                order.Payed += money;
                if (order.Total <= order.Payed)
                {
                    order.IsPayment = true;
                    order.Descreption = "Số tiền nợ " + money + " đã được thanh toán đủ.";
                }
                Update(order);
                CommitChange();

                mess = "Trả nợ thành công.";
                return true;
            }
            catch (Exception ex)
            {
                log.TryLog(ex);
                mess = "lỗi hệ thống, vui lòng thử lại sau.";
                return false;
            }
        }
    }
}

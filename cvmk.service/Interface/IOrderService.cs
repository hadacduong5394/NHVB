using cvmk.context.domain;
using hddata.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.service.Interface
{
    public interface IOrderService: IBaseService<Order, int>
    {
        IList<Order> GetbyFilter(string code, int currentPage, int pageSize);

        IList<Order> GetbyFilter(int comId, string code, DateTime? start, DateTime? end, bool? isPayment, int currentPage,int pageSize, out int total);

        bool OrderProduct(int tableId,Order order, IList<OrderDetail> orderdetails, out string message, out int orderId);

        bool Delete(int id, out string message);

        Order GetCurrentOrder(int tableId);

        bool AddProductToOrder(int orderId, OrderDetail orderdetail, out string message);

        bool ChangeQuantityProduct(int orderId, int detailId, int quantity, out string message);

        bool DeleteDetail(int orderId, int detailId, out string message);

        bool CancleOrder(int orderId, out string message);

        bool Payment(Order order, int tableId, out string message);

        bool ChangeTable(int orderId, int toTableId, out string mess);

        bool CombineTable(int fromOrderId, int toTableId, out int workingOrderId, out string mess);

        bool PayBack(int orderId, decimal money, out string mess);
    }
}

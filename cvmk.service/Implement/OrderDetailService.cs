using cvmk.context.domain;
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
    public class OrderDetailService: BaseService<OrderDetail, int>, IOrderDetailService
    {
        private readonly IErrorService log;
        public OrderDetailService(IDbFactory dbFactory): base(dbFactory)
        {
            log = IoC.Resolve<IErrorService>();
        }

        public IList<OrderDetail> GetByOrderId(int orderId)
        {
            return Query.Where(n => n.OrderId == orderId).OrderBy(n => n.Id).ToList();
        }
    }
}

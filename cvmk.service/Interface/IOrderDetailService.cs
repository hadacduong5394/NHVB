using cvmk.context.domain;
using hddata.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.service.Interface
{
    public interface IOrderDetailService: IBaseService<OrderDetail, int>
    {
        IList<OrderDetail> GetByOrderId(int orderId);
    }
}

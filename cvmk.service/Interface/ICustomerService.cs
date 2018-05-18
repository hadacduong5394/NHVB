using cvmk.context.domain;
using hddata.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.service.Interface
{
    public interface ICustomerService : IBaseService<Customer, int>
    {
        IList<Customer> GetbyFilter(int com_id, string code, string name, string taxcode, string email, int currentPage, int pageSize, out int total);

        bool Create(Customer entity, out string message);

        bool Update(Customer entity, out string message);

        bool Delete(int id, out string message);
    }
}

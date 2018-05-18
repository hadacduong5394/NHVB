using cvmk.context.domain;
using hddata.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.service.Interface
{
    public interface ISupplierService: IBaseService<Supplier, int>
    {
        IList<Supplier> GetbyFilter(int com_id, string name, string email, string taxcode, string phonenumber, int currentPage, int pageSize, out int total);

        bool Create(Supplier entity, out string message);

        bool Update(Supplier entity, out string message);

        bool Delete(int id, out string message);

        Supplier GetbyCode(string code);
    }
}

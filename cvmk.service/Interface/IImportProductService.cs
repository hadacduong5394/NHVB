using cvmk.context.domain;
using hddata.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.service.Interface
{
    public interface IImportProductService: IBaseService<ImportProduct, int>
    {
        IList<ImportProduct> GetbyFilter(int com_id, string code, int currentPage, int pageSize, out int total);

        bool Create(ImportProduct entity, IList<ImportProductDetail> details, out string message);

        bool Update(ImportProduct entity, IList<ImportProductDetail> details, out string message);

        bool Delete(int id, out string message);
    }
}

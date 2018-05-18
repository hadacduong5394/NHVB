using cvmk.context.domain;
using cvmk.service.Interface;
using hddata.DBFactory;
using hddata.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.service.Implement
{
    public class ImportProductDetailService: BaseService<ImportProductDetail, int>, IImportProductDetailService
    {
        public ImportProductDetailService(IDbFactory dbFactory): base(dbFactory)
        {
        }
    }
}

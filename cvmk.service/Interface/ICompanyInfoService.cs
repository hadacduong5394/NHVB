using hdcontext.AdminDomain.Domain;
using hddata.RepositoryPattern;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace cvmk.service.Interface
{
    public interface ICompanyInfoService : IBaseService<CompanyInfo, int>
    {
        bool Create(CompanyInfo entity, out string message);

        bool Update(CompanyInfo entity, out string message);

        bool Delete(int id, out string message);

        IList<CompanyInfo> GetCompanies();

        bool ChangeInfo(CompanyInfo entity, out string message);

    }
}
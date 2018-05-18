using hdcontext.AdminDomain.IdentityCode;
using hddata.RepositoryPattern;

namespace cvmk.service.Interface
{
    public interface IImportProductCodeService : IBaseService<ImportProductCode, int>
    {
        bool GenCode(out string result);
    }
}
using hdcontext.AdminDomain.IdentityCode;
using hddata.RepositoryPattern;

namespace cvmk.service.Interface
{
    public interface ISupplierCodeService : IBaseService<SupplierCode, int>
    {
        bool GenCode(out string result);
    }
}
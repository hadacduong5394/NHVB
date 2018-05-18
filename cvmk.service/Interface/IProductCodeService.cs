using hdcontext.AdminDomain.IdentityCode;
using hddata.RepositoryPattern;

namespace cvmk.service.Interface
{
    public interface IProductCodeService : IBaseService<ProductCode, int>
    {
        bool GenCode(out string result);
    }
}
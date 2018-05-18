using hdcontext.AdminDomain.IdentityCode;
using hddata.RepositoryPattern;

namespace cvmk.service.Interface
{
    public interface IMaterialCodeService : IBaseService<MaterialCode, int>
    {
        bool GenCode(out string result);
    }
}
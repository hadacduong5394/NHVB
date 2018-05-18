using hdcontext.AdminDomain.IdentityCode;
using hddata.RepositoryPattern;

namespace cvmk.service.Interface
{
    public interface IOrderCodeService : IBaseService<OrderCode, int>
    {
        bool GenCode(out string result);
    }
}
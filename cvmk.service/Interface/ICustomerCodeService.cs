using hdcontext.AdminDomain.IdentityCode;
using hddata.RepositoryPattern;

namespace cvmk.service.Interface
{
    public interface ICustomerCodeService : IBaseService<CustomerCode, int>
    {
        bool GenCode(out string result);
    }
}
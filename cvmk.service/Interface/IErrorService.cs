using hdcontext.IdentityDomain;
using hddata.RepositoryPattern;
using System;

namespace cvmk.service.Interface
{
    public interface IErrorService : IBaseService<Error, int>
    {
        bool TryLog(Exception ex);
    }
}
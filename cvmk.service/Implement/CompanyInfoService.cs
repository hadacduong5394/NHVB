using cvmk.service.Interface;
using hdcontext.AdminDomain.Domain;
using hdcore;
using hdcore.Utils;
using hddata.DBFactory;
using hddata.RepositoryPattern;
using System;
using System.Linq;
using System.Collections.Generic;
using cvmk.context.IdentityConfiguration;

namespace cvmk.service.Implement
{
    public class CompanyInfoService : BaseService<CompanyInfo, int>, ICompanyInfoService
    {
        private readonly IErrorService _log;
        public CompanyInfoService(IDbFactory dbFactory) : base(dbFactory)
        {
            _log = IoC.Resolve<IErrorService>();
        }

        public bool ChangeInfo(CompanyInfo entity, out string message)
        {
            try
            {
                Update(entity);
                CommitChange();
                message = "đổi thông tin thành công.";
                return true;
            }
            catch (Exception ex)
            {
                _log.TryLog(ex);
                message = "Lỗi hệ thống.";
                return false;
            }
        }

        public bool Create(CompanyInfo entity, out string message)
        {
            var errSrv = IoC.Resolve<IErrorService>();
            try
            {
                CreateNew(entity);
                CommitChange();
                message = TextHelper.CREAT_SUCCESSFULL;
                return true;
            }
            catch (Exception ex)
            {
                errSrv.TryLog(ex);
                message = TextHelper.ERROR_SYSTEM;
                return false;
            }
        }

        public bool Delete(int id, out string message)
        {
            var errSrv = IoC.Resolve<IErrorService>();
            try
            {
                Delete(id);
                CommitChange();
                message = TextHelper.DELETE_SUCCESSFULL;
                return true;
            }
            catch (Exception ex)
            {
                errSrv.TryLog(ex);
                message = TextHelper.ERROR_SYSTEM;
                return false;
            }
        }

        public IList<CompanyInfo> GetCompanies()
        {
            return Query.Where(n => n.Status == true).OrderBy(n => n.Id).ToList();
        }

        public bool Update(CompanyInfo entity, out string message)
        {
            var errSrv = IoC.Resolve<IErrorService>();
            try
            {
                Update(entity);
                CommitChange();
                message = TextHelper.EDIT_SUCCESSFULL;
                return true;
            }
            catch (Exception ex)
            {
                errSrv.TryLog(ex);
                message = TextHelper.ERROR_SYSTEM;
                return false;
            }
        }
    }
}
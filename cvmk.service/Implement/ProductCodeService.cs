﻿using cvmk.service.Helper;
using cvmk.service.Interface;
using hdcontext.AdminDomain.IdentityCode;
using hdcore;
using hddata.DBFactory;
using hddata.RepositoryPattern;
using System;

namespace cvmk.service.Implement
{
    public class ProductCodeService : BaseService<ProductCode, int>, IProductCodeService
    {
        private readonly IErrorService log;

        public ProductCodeService(IDbFactory dbFactory) : base(dbFactory)
        {
            log = IoC.Resolve<IErrorService>();
        }

        public bool GenCode(out string result)
        {
            BeginTran();
            try
            {
                var entity = new ProductCode()
                {
                    KeyCode = KeyCode.KEY_CODE_PRODUCT
                };
                var t = CreateNew(entity);
                CommitChange();

                t.KeyCode = t.KeyCode + "." + t.Id;
                Update(t);
                CommitChange();

                CommitTran();
                result = t.KeyCode;
                return true;
            }
            catch (Exception ex)
            {
                RollbackTran();
                log.TryLog(ex);
                result = ex.Message;
                return false;
            }
        }
    }
}
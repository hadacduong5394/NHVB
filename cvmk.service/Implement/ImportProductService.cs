using cvmk.context.domain;
using cvmk.context.IdentityConfiguration;
using cvmk.service.Interface;
using hdcore;
using hddata.DBFactory;
using hddata.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cvmk.service.Implement
{
    public class ImportProductService : BaseService<ImportProduct, int>, IImportProductService
    {
        private readonly IErrorService log;

        public ImportProductService(IDbFactory dbFactory) : base(dbFactory)
        {
            log = IoC.Resolve<IErrorService>();
        }

        public bool Create(ImportProduct entity, IList<ImportProductDetail> details, out string message)
        {
            BeginTran();
            try
            {
                entity.CreateBy = CurrentUser.Instance.User.UserName;
                entity.CreateDate = DateTime.Now;
                entity.ComId = CurrentUser.Instance.User.ComId;
                CreateNew(entity);
                CommitChange();

                var detailSrv = IoC.Resolve<IImportProductDetailService>();
                var mtSrv = IoC.Resolve<IMaterialService>();
                foreach (var detail in details)
                {
                    var mt = mtSrv.GetbyKey(detail.MaterialId);
                    mt.Quantity += detail.Quantity;
                    mt.RootPrice = (int)detail.Amount;
                    mtSrv.Update(mt);

                    detail.ImportProductId = entity.Id;
                    detail.MaterialName = mt.Name;
                    detail.MaterialCode = mt.Code;
                    detailSrv.CreateNew(detail);
                }
                detailSrv.CommitChange();

                CommitTran();
                message = hdcore.Utils.TextHelper.CREAT_SUCCESSFULL;
                return true;
            }
            catch (Exception ex)
            {
                RollbackTran();
                log.TryLog(ex);
                message = hdcore.Utils.TextHelper.ERROR_SYSTEM;
                return false;
            }
        }

        public bool Delete(int id, out string message)
        {
            BeginTran();
            try
            {
                var detailSrv = IoC.Resolve<IImportProductDetailService>();
                detailSrv.DeleteMulti(n => n.ImportProductId == id);
                detailSrv.CommitChange();
                Delete(id);
                CommitChange();

                CommitTran();
                message = hdcore.Utils.TextHelper.DELETE_SUCCESSFULL;
                return true;
            }
            catch (Exception ex)
            {
                log.TryLog(ex);
                message = hdcore.Utils.TextHelper.ERROR_SYSTEM;
                return false;
            }
        }

        public IList<ImportProduct> GetbyFilter(int com_id, string code, int currentPage, int pageSize, out int total)
        {
            var query = Query.Where(n => n.ComId == com_id);
            if (!string.IsNullOrEmpty(code))
            {
                query = query.Where(n => n.Code.Contains(code));
            }
            query = query.OrderByDescending(n => n.Id);
            total = query.Count();

            return query.Skip(currentPage * pageSize).Take(pageSize).ToList();
        }

        public bool Update(ImportProduct entity, IList<ImportProductDetail> details, out string message)
        {
            BeginTran();
            try
            {
                var detailSrv = IoC.Resolve<IImportProductDetailService>();
                var mtSrv = IoC.Resolve<IMaterialService>();
                detailSrv.DeleteMulti(n => n.ImportProductId == entity.Id);
                detailSrv.CommitChange();

                entity.ModifyBy = CurrentUser.Instance.User.UserName;
                entity.ModifyDate = DateTime.Now;
                Update(entity);
                CommitChange();

                foreach (var detail in details)
                {
                    var prod = mtSrv.GetbyKey(detail.MaterialId);
                    detail.ImportProductId = entity.Id;
                    detail.MaterialName = prod.Name;
                    detail.MaterialCode = prod.Code;
                    detailSrv.CreateNew(detail);
                }
                detailSrv.CommitChange();

                CommitTran();
                message = hdcore.Utils.TextHelper.EDIT_SUCCESSFULL;
                return true;
            }
            catch (Exception ex)
            {
                RollbackTran();
                log.TryLog(ex);
                message = hdcore.Utils.TextHelper.ERROR_SYSTEM;
                return false;
            }
        }
    }
}
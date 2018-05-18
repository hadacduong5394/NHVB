using cvmk.context.domain.Products;
using cvmk.context.IdentityConfiguration;
using cvmk.service.Interface;
using hdcore;
using hddata.DBFactory;
using hddata.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cvmk.context.domain.Material;

namespace cvmk.service.Implement
{
    public class ProductService : BaseService<Product, int>, IProductService
    {
        private readonly IErrorService log;
        public ProductService(IDbFactory dbFactory) : base(dbFactory)
        {
            log = IoC.Resolve<IErrorService>();
        }

        public bool Create(Product product, IList<ProductMaterials> materials, IList<ProductPropertis> lstProp, out string message)
        {
            try
            {
                if (Query.Any(n => n.BarCode.Equals(product.BarCode)))
                {
                    message = "Mã sản phẩm này đã tồn tại.";
                    return false;
                }
                BeginTran();
                var propSrv = IoC.Resolve<IProductPropertisService>();
                product.CreateBy = CurrentUser.Instance.User.UserName;
                product.CreateDate = DateTime.Now;
                product.ComId = CurrentUser.Instance.User.ComId;
                CreateNew(product);
                CommitChange();

                foreach (var item in lstProp)
                {
                    item.ProductId = product.Id;
                    propSrv.CreateNew(item);
                }
                var pmSrv = IoC.Resolve<IProductMaterialService>();
                foreach (var item in materials)
                {
                    item.ProductId = product.Id;
                    pmSrv.CreateNew(item);
                }
                CommitChange();
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
            try
            {
                BeginTran();
                var propSrv = IoC.Resolve<IProductPropertisService>();
                propSrv.DeleteMulti(n => n.ProductId == id);

                var pmSrv = IoC.Resolve<IProductMaterialService>();
                pmSrv.DeleteMulti(n => n.ProductId == id);

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

        public IList<Product> GetbyFilter(int com_id, string name, string code, int currentPage, int pageSize, out int total)
        {
            var query = Query.Where(n => n.ComId == com_id);
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(n => n.Name.Contains(name));
            }
            if (!string.IsNullOrEmpty(code))
            {
                query = query.Where(n => n.BarCode.Contains(code));
            }
            query = query.OrderByDescending(n => n.Id);
            total = query.Count();
            return query.Skip(currentPage * pageSize).Take(pageSize).ToList();
        }

        public IList<Product> GetMenuProductFilter(int com_id, string code_name, int floorId, int groupId, int typeId, int currentPage, int pageSize, out int total)
        {
            var query = Query.Where(n => n.Status == true && n.ComId == com_id);
            var floor = IoC.Resolve<IFloorService>().GetbyKey(floorId);
            if (floor.VIP)
            {
                query = query.Where(n => n.VIP == true);
            }
            else
            {
                query = query.Where(n => n.VIP == false);
            }
            if (!string.IsNullOrEmpty(code_name))
            {
                query = query.Where(n => n.BarCode.Contains(code_name) || n.Name.Contains(code_name));
            }
            if (groupId != -1)
            {
                query = query.Where(n => n.GroupId == groupId);
            }
            if (typeId != -1)
            {
                query = query.Where(n => n.TypeId == typeId);
            }
            query = query.OrderByDescending(n => n.Id);
            total = query.Count();
            return query.Skip(currentPage * pageSize).Take(pageSize).ToList();
        }

        public bool Update(Product product, IList<ProductMaterials> materials, IList<ProductPropertis> lstProp, out string message)
        {
            try
            {
                if (Query.Any(n => n.Id != product.Id && n.BarCode.Equals(product.BarCode)))
                {
                    message = "Mã sản phẩm này đã tồn tại.";
                    return false;
                }
                BeginTran();
                var propSrv = IoC.Resolve<IProductPropertisService>();
                propSrv.DeleteMulti(n => n.ProductId == product.Id);
                propSrv.CommitChange();

                var pmSrv = IoC.Resolve<IProductMaterialService>();
                pmSrv.DeleteMulti(n => n.ProductId == product.Id);
                pmSrv.CommitChange();

                product.ModifyBy = CurrentUser.Instance.User.UserName;
                product.ModifyDate = DateTime.Now;
                Update(product);

                foreach (var item in lstProp)
                {
                    item.ProductId = product.Id;
                    propSrv.CreateNew(item);
                }

                foreach (var item in materials)
                {
                    item.ProductId = product.Id;
                    pmSrv.CreateNew(item);
                }
                CommitChange();
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

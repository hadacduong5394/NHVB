using cvmk.context.IdentityConfiguration;
using cvmk.service.Interface;
using cvmksite.Models.ViewModel;
using hdcore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace cvmksite.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/product")]
    public class ProductController : ShareApiController
    {
        [Route("getbyfilter")]
        [HttpGet]
        public HttpResponseMessage GetbyFilter(HttpRequestMessage request, string name, string code, int page = 0, int pageSize = 10)
        {
            int total = 0;
            int com_id = CurrentUser.Instance.User.ComId;
            var items = IoC.Resolve<IProductService>().GetbyFilter(com_id, name, code, page, pageSize, out total).Select(n => new ProductViewModel
            {
                Id = n.Id,
                BarCode = n.BarCode,
                Descreption = n.Descreption,
                Name = n.Name,
                Quantity  =n.Quantity,
                Status = n.Status,
                VIP = n.VIP,
                Unit = n.Unit,
                RootPrice = n.RootPrice,
                Price = n.Price,
                Image = n.Image
            }).ToList();
            var result = new PaginationSet<ProductViewModel>()
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                Items = items
            };
            return request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("genarecode")]
        public HttpResponseMessage GenareCode(HttpRequestMessage request)
        {
            string rs = string.Empty;
            if (IoC.Resolve<IProductCodeService>().GenCode(out rs))
            {
                return request.CreateResponse(HttpStatusCode.OK, new { code = rs });
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, new { mess = "lỗi hệ thống, vui lòng thử lại sau." });
        }

        [HttpPost]
        [Route("create")]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductViewModel vm)
        {
            string mess = "";
            var model = vm.UpdateModel();
            var lstProp = vm.DeserializeProductPropertis();
            var lstMaterials = vm.DeserializeProductMaterials();
            if (IoC.Resolve<IProductService>().Create(model, lstMaterials, lstProp, out mess))
            {
                return request.CreateResponse(HttpStatusCode.OK, mess);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [Route("getbykey")]
        [HttpGet]
        public HttpResponseMessage GetbyKey(HttpRequestMessage request, int id)
        {
            var entity = IoC.Resolve<IProductService>().GetbyKey(id);
            var rs = new ProductViewModel
            {
                Id = entity.Id,
                BarCode = entity.BarCode,
                Name = entity.Name,
                Content = entity.Content,
                Descreption = entity.Descreption,
                GroupId = entity.GroupId,
                Image = entity.Image,
                Images = entity.Images,
                Quantity = entity.Quantity,
                RootPrice = entity.RootPrice,
                Price = entity.Price,
                Status = entity.Status,
                VIP = entity.VIP,
                TypeId = entity.TypeId,
                Unit = entity.Unit,
                CreateBy = entity.CreateBy,
                CreateDate = entity.CreateDate.ToString("dd/MM/yyyy"),
                ModifyBy = entity.ModifyBy,
                ModifyDate = entity.ModifyDate.HasValue ? entity.ModifyDate.Value.ToString("dd/MM/yyyy") : string.Empty
            };
            rs.SerializeProductPropertis(IoC.Resolve<IProductPropertisService>().GetMulti(n => n.ProductId == id).ToList());
            rs.SerializeProductMaterials(IoC.Resolve<IProductMaterialService>().GetMulti(n => n.ProductId == id).ToList());
            return request.CreateResponse(HttpStatusCode.OK, rs);
        }

        [HttpPut]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductViewModel vm)
        {
            string mess = "";
            var entity = IoC.Resolve<IProductService>().GetbyKey(vm.Id);
            var model = vm.UpdateModel(entity);
            var lstProp = vm.DeserializeProductPropertis();
            var lstMaterials = vm.DeserializeProductMaterials();
            if (IoC.Resolve<IProductService>().Update(model, lstMaterials, lstProp, out mess))
            {
                return request.CreateResponse(HttpStatusCode.OK, mess);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            string mess = "";
            if (IoC.Resolve<IProductService>().Delete(id, out mess))
            {
                return request.CreateResponse(HttpStatusCode.OK, mess);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [Route("getall")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            int com_id = CurrentUser.Instance.User.ComId;
            return request.CreateResponse(HttpStatusCode.OK, IoC.Resolve<IProductService>().GetMulti(n => n.Status == true && n.ComId == com_id));
        }
    }
}

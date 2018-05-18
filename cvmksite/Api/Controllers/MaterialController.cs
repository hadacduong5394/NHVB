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
    [RoutePrefix("api/material")]
    public class MaterialController : ShareApiController
    {
        [HttpGet]
        [Route("getbyfilter")]
        public HttpResponseMessage GetbyFilter(HttpRequestMessage request, string code, string name, int page = 0, int pageSize = 10)
        {
            var total = 0;
            var materialSrv = IoC.Resolve<IMaterialService>();
            var com_id = CurrentUser.Instance.User.ComId;
            var items = materialSrv.GetbyFilter(com_id, code, name, page, pageSize, out total).Select(n => new MaterialViewModel
            {
                Id = n.Id,
                Code = n.Code,
                Name = n.Name,
                Quantity = n.Quantity,
                RootPrice = n.RootPrice,
                Unit = n.Unit,
                Status = n.Status,
                Image = n.Image
            }).ToList();

            var result = new PaginationSet<MaterialViewModel>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };

            return request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("getmaterials")]
        public HttpResponseMessage GetMaterials(HttpRequestMessage request, string keyword, int page = 0, int pageSize = 10)
        {
            var total = 0;
            var materialSrv = IoC.Resolve<IMaterialService>();
            var com_id = CurrentUser.Instance.User.ComId;
            var items = materialSrv.GetMaterials(com_id, keyword, page, pageSize, out total).Select(n => new MaterialViewModel
            {
                Id = n.Id,
                Code = n.Code,
                Name = n.Name,
                Image = n.Image,
                Descreption = n.Descreption,
                Unit = n.Unit,
                RootPrice = n.RootPrice
            }).ToList();

            var result = new PaginationSet<MaterialViewModel>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };

            return request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("getbykey")]
        public HttpResponseMessage GetbyKey(HttpRequestMessage request, int id)
        {
            var materialSrv = IoC.Resolve<IMaterialService>();
            var entity = materialSrv.GetbyKey(id);
            var result = new MaterialViewModel
            {
                Id = entity.Id,
                Code = entity.Code,
                ComId = entity.ComId,
                Descreption = entity.Descreption,
                Image = entity.Image,
                Name = entity.Name,
                Quantity = entity.Quantity,
                RootPrice = entity.RootPrice,
                Status = entity.Status,
                Unit = entity.Unit,
                CreateBy = entity.CreateBy,
                CreateDate = entity.CreateDate.ToString("dd/MM/yyyy"),
                ModifyBy = entity.ModifyBy,
                ModifyDate = entity.ModifyDate.HasValue ? entity.ModifyDate.Value.ToString("dd/MM/yyyy") : string.Empty
            };
            return request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("create")]
        public HttpResponseMessage Create(HttpRequestMessage request, MaterialViewModel vm)
        {
            string mess = "";
            var materialSrv = IoC.Resolve<IMaterialService>();
            var model = vm.UpdateModel();
            if (materialSrv.Create(model, out mess))
            {
                return request.CreateResponse(HttpStatusCode.OK, mess);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [HttpPut]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, MaterialViewModel vm)
        {
            string mess = "";
            var materialSrv = IoC.Resolve<IMaterialService>();
            var model = vm.UpdateModel(vm.Id);
            if (materialSrv.Edit(model, out mess))
            {
                return request.CreateResponse(HttpStatusCode.OK, mess);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [HttpDelete]
        [Route("delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            string mess = "";
            var materialSrv = IoC.Resolve<IMaterialService>();
            if (materialSrv.Delete(id, out mess))
            {
                return request.CreateResponse(HttpStatusCode.OK, mess);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, mess);
        }

        [HttpGet]
        [Route("getmaterialsforimport")]
        public HttpResponseMessage GetMaterialForImport(HttpRequestMessage request)
        {
            var mtSrv = IoC.Resolve<IMaterialService>();
            int com_id = CurrentUser.Instance.User.ComId;
            var result = mtSrv.GetMaterialForImport(com_id).Select(n => new MaterialViewModel {
                Id = n.Id,
                Name = n.Name,
                Code = n.Code
            }).ToList();
            return request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("genarecode")]
        public HttpResponseMessage GenareCode(HttpRequestMessage request)
        {
            string rs = string.Empty;
            if (IoC.Resolve<IMaterialCodeService>().GenCode(out rs))
            {
                return request.CreateResponse(HttpStatusCode.OK, new { code = rs });
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, new { mess = "lỗi hệ thống, vui lòng thử lại sau." });
        }
    }
}

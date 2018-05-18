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
    [RoutePrefix("api/importproduct")]
    public class ImportProductController : ShareApiController
    {
        [HttpGet]
        [Route("getbyfilter")]
        public HttpResponseMessage GetbyFilter(HttpRequestMessage request, string code, int page = 0, int pageSize = 10)
        {
            int total = 0;
            int com_id = CurrentUser.Instance.User.ComId;
            var items = IoC.Resolve<IImportProductService>().GetbyFilter(com_id, code, page, pageSize, out total).Select(n => new ImportProductViewModel {
                Id  = n.Id,
                AddressSupplier = n.AddressSupplier,
                Code = n.Code,
                Descreption = n.Descreption,
                EmailSupplier = n.EmailSupplier,
                ImportDate = n.ImportDate.ToString("dd/MM/yyyy"),
                PhoneSupplier = n.PhoneSupplier,
                SuppierCode = n.SuppierCode,
                SupplierName = n.SupplierName,
                SupplierTaxcode = n.SupplierTaxcode,
                UserId = n.UserId,
                Status = n.Status,
                TotalAmount = n.TotalAmount,
                VAT = n.VAT
            }).ToList();
            var result = new PaginationSet<ImportProductViewModel>
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
            if (IoC.Resolve<IImportProductCodeService>().GenCode(out rs))
            {
                return request.CreateResponse(HttpStatusCode.OK, new { code = rs });
            }
            return request.CreateResponse(HttpStatusCode.BadRequest, new { mess = "lỗi hệ thống, vui lòng thử lại sau." });
        }

        [HttpPost]
        [Route("create")]
        public HttpResponseMessage Create(HttpRequestMessage request, ImportProductViewModel vm)
        {
            try
            {
                var lstDetailVm = vm.DeserializeDetails();
                var model = vm.UpdateModel();
                string message = "";
                var srv = IoC.Resolve<IImportProductService>();
                if (srv.Create(model, ImportProductDetailViewModel.UpdateModels(lstDetailVm), out message))
                {
                    return request.CreateResponse(HttpStatusCode.OK, message);
                }

                return request.CreateResponse(HttpStatusCode.BadRequest, message);
            }
            catch (Exception ex)
            {
                IoC.Resolve<IErrorService>().TryLog(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, hdcore.Utils.TextHelper.ERROR_SYSTEM);
            }
        }

        [HttpGet]
        [Route("getbykey")]
        public HttpResponseMessage GetbyKey(HttpRequestMessage request, int id)
        {
            var ipSrv = IoC.Resolve<IImportProductService>();
            var ipDetailSrv = IoC.Resolve<IImportProductDetailService>();
            var entity = ipSrv.GetbyKey(id);
            var result = new ImportProductViewModel
            {
                Id = entity.Id,
                Code = entity.Code,
                AddressSupplier = entity.AddressSupplier,
                Descreption = entity.Descreption,
                Status = entity.Status,
                SuppierCode = entity.SuppierCode,
                TotalAmount = entity.TotalAmount,
                VAT = entity.VAT,
                EmailSupplier = entity.EmailSupplier,
                ImportDate = entity.ImportDate.ToString("dd/MM/yyyy"),
                SupplierName = entity.SupplierName,
                PhoneSupplier = entity.PhoneSupplier,
                SupplierTaxcode = entity.SupplierTaxcode,
                UserId = entity.UserId
            };
            result.serializeDetails();
            return request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPut]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, ImportProductViewModel vm)
        {
            try
            {
                var lstDetailVm = vm.DeserializeDetails();
                var model = vm.UpdateModel(IoC.Resolve<IImportProductService>().GetbyKey(vm.Id));
                string message = "";
                var srv = IoC.Resolve<IImportProductService>();
                if (srv.Update(model, ImportProductDetailViewModel.UpdateModels(lstDetailVm), out message))
                {
                    return request.CreateResponse(HttpStatusCode.OK, message);
                }

                return request.CreateResponse(HttpStatusCode.BadRequest, message);
            }
            catch (Exception ex)
            {
                IoC.Resolve<IErrorService>().TryLog(ex);
                return request.CreateResponse(HttpStatusCode.BadRequest, hdcore.Utils.TextHelper.ERROR_SYSTEM);
            }
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            string message = "";
            var srv = IoC.Resolve<IImportProductService>();
            if (srv.Delete(id, out message))
            {
                return request.CreateResponse(HttpStatusCode.OK, message);
            }

            return request.CreateResponse(HttpStatusCode.BadRequest, message);
        }
    }
}

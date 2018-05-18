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
    [RoutePrefix("api/menuproduct")]
    [Authorize]
    public class MenuProductController : ShareApiController
    {
        [HttpGet]
        [Route("getmenuproducts")]
        public HttpResponseMessage GetMenuProduct(HttpRequestMessage request, string code_name, int floorId, int groupId = -1, int typeId = -1, int page = 0, int pageSize = 16)
        {
            var productSrv = IoC.Resolve<IProductService>();
            int total = 0;
            int com_id = CurrentUser.Instance.User.ComId;
            var items = productSrv.GetMenuProductFilter(com_id, code_name, floorId, groupId, typeId, page, pageSize, out total).Select(n => new MenuProductViewModel {
                Id = n.Id,
                BarCode = n.BarCode,
                Name = n.Name,
                Image = n.Image,
                MoreImages = n.Images,
                Price = n.Price
            }).ToList();
            var result = new PaginationSet<MenuProductViewModel>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };

            return request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}

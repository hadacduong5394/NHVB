using cvmk.context.domain;
using cvmk.service.Interface;
using hdcore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cvmksite.Models.ViewModel
{
    public class ImportProductDetailViewModel
    {
        public int Id { get; set; }
        public int ImportProductId { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Descreption { get; set; }
        public ImportProductDetail UpdateModel()
        {
            var proSrv = IoC.Resolve<IMaterialService>().GetbyKey(this.ProductId);
            return new ImportProductDetail
            {
                Id = this.Id,
                Amount = this.Amount,
                Descreption = this.Descreption,
                ImportProductId = this.ImportProductId,
                MaterialId = this.ProductId,
                MaterialCode  = proSrv.Code,
                MaterialName = proSrv.Name,
                Quantity = this.Quantity,
                TotalAmount = this.TotalAmount
            };
        }
        public static IList<ImportProductDetail> UpdateModels(IList<ImportProductDetailViewModel> lst)
        {
            var result = new List<ImportProductDetail>();
            foreach (var item in lst)
            {
                var proSrv = IoC.Resolve<IMaterialService>().GetbyKey(item.ProductId);
                result.Add(new ImportProductDetail {
                    Id = item.Id,
                    Amount = item.Amount,
                    Descreption = item.Descreption,
                    ImportProductId = item.ImportProductId,
                    MaterialId = item.ProductId,
                    MaterialCode = proSrv.Code,
                    MaterialName = proSrv.Name,
                    Quantity = item.Quantity,
                    TotalAmount = item.TotalAmount
                });
            }

            return result;
        }
    }
}
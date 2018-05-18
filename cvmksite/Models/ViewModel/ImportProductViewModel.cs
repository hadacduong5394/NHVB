using cvmk.context.domain;
using cvmk.context.IdentityConfiguration;
using cvmk.service.Interface;
using hdcore;
using hdidentity.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cvmksite.Models.ViewModel
{
    public class ImportProductViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string UserId { get; set; } = CurrentUser.Instance.User.Id;
        public string UserName { get { return IoC.Resolve<IUserService>().GetbyKey(this.UserId).UserName; } }
        public int VAT { get; set; }
        public decimal TotalAmount { get; set; }
        public string SuppierCode { get; set; }
        public string SupplierName { get; set; }
        public string SupplierTaxcode { get; set; }
        public string EmailSupplier { get; set; }
        public string AddressSupplier { get; set; }
        public string PhoneSupplier { get; set; }
        public string ImportDate { get; set; }
        public string Descreption { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public bool Status { get; set; }
        public string JsonDetails { get; set; }
        public IList<ImportProductDetailViewModel> Details
        {
            get
            {
                var detailSrv = IoC.Resolve<IImportProductDetailService>();
                return detailSrv.GetMulti(n => n.ImportProductId == this.Id).OrderByDescending(n => n.Id).Select(n => new ImportProductDetailViewModel {
                    Id = n.Id,
                    ImportProductId = n.ImportProductId,
                    Amount = n.Amount,
                    Descreption = n.Descreption,
                    ProductCode = n.MaterialCode,
                    ProductId = n.MaterialId,
                    ProductName = n.MaterialName,
                    Quantity = n.Quantity,
                    TotalAmount = n.TotalAmount,
                }).ToList();
            }
        }
        public ImportProduct UpdateModel()
        {
            var supplierSrv = IoC.Resolve<ISupplierService>();
            var supp = supplierSrv.GetbyCode(this.SuppierCode);
            return new ImportProduct()
            {
                Id = this.Id,
                Code = this.Code,
                UserId = CurrentUser.Instance.User.Id,
                SuppierCode = this.SuppierCode,
                AddressSupplier = supp.Address,
                Descreption = this.Descreption,
                EmailSupplier = supp.Email,
                ImportDate = DateTime.Now,
                SupplierName = supp.Name,
                PhoneSupplier = supp.PhoneNumber,
                VAT = this.VAT,
                SupplierTaxcode = supp.TaxCode,
                TotalAmount = this.TotalAmount,
                Status = this.Status,
                CreateBy = CurrentUser.Instance.User.UserName,
                CreateDate = DateTime.Now,
                ComId = CurrentUser.Instance.User.ComId
            };
        }
        public ImportProduct UpdateModel(ImportProduct entity)
        {
            entity.Id = this.Id;
            entity.UserId = CurrentUser.Instance.User.Id;
            entity.SuppierCode = this.SuppierCode;
            entity.AddressSupplier = this.AddressSupplier;
            entity.Descreption = this.Descreption;
            entity.EmailSupplier = this.EmailSupplier;
            entity.ImportDate = DateTime.Now;
            entity.SupplierName = this.SupplierName;
            entity.PhoneSupplier = this.PhoneSupplier;
            entity.SupplierTaxcode = this.SupplierTaxcode;
            entity.VAT = this.VAT;
            entity.TotalAmount = this.TotalAmount;
            entity.Status = this.Status;
            entity.ModifyBy = CurrentUser.Instance.User.UserName;
            entity.ModifyDate = DateTime.Now;
            return entity;
        }
        public IList<ImportProductDetailViewModel> DeserializeDetails()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<IList<ImportProductDetailViewModel>>(this.JsonDetails);
        }
        public void serializeDetails()
        {
            JsonDetails = Newtonsoft.Json.JsonConvert.SerializeObject(this.Details);
        }
    }
}
using cvmk.context.domain;
using cvmk.context.IdentityConfiguration;
using System;

namespace cvmksite.Models.ViewModel
{
    public class SupplierViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Logo { get; set; }
        public string Descreption { get; set; }
        public string TaxCode { get; set; }
        public string PhoneNumber { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public bool Status { get; set; }
        public Supplier UpdateModel()
        {
            return new Supplier
            {
                Id = this.Id,
                Address = this.Address,
                Code = this.Code,
                CreateBy = CurrentUser.Instance.User.UserName,
                CreateDate = DateTime.Now,
                Descreption = this.Descreption,
                Email = this.Email,
                Logo = this.Logo,
                Name = this.Name,
                PhoneNumber = this.PhoneNumber,
                Status = this.Status,
                TaxCode = this.TaxCode
            };
        }
        public Supplier UpdateModel(Supplier entity)
        {
            entity.Address = this.Address;
            entity.Code = this.Code;
            entity.Descreption = this.Descreption;
            entity.Email = this.Email;
            entity.Logo = this.Logo;
            entity.PhoneNumber = this.PhoneNumber;
            entity.Status = this.Status;
            entity.TaxCode = this.TaxCode;
            entity.ModifyBy = CurrentUser.Instance.User.UserName;
            entity.ModifyDate = DateTime.Now;
            return entity;
        }
    }
}
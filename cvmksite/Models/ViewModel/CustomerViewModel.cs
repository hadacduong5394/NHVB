using cvmk.context.domain;
using cvmk.context.IdentityConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cvmksite.Models.ViewModel
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public int Type { get; set; } = 1; //1 Cá nhân, 2 công ty
        public string Name { get; set; }
        public string Company { get; set; }
        public string Code { get; set; }
        public string BirthDay { get; set; }
        public string TaxCode { get; set; }
        public string Avatar { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Descreption { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public bool Status { get; set; }
        public Customer UpdateModel()
        {
            return new Customer
            {
                Id = this.Id,
                Type = this.Type,
                Name = this.Name,
                Company = this.Company,
                Code = this.Code,
                BirthDay = !string.IsNullOrEmpty(this.BirthDay) ? DateTime.Parse(this.BirthDay) : (DateTime?)null,
                TaxCode = this.TaxCode,
                Avatar = this.Avatar,
                PhoneNumber = this.PhoneNumber,
                Email = this.Email,
                Address = this.Address,
                Descreption = this.Descreption,
                CreateBy = CurrentUser.Instance.User.UserName,
                CreateDate = DateTime.Now,
                Status = this.Status,
                ComId = CurrentUser.Instance.User.ComId
            };
        }
        public Customer UpdateModel(Customer entity)
        {
            entity.Id = this.Id;
            entity.Type = this.Type;
            entity.Name = this.Name;
            entity.Company = this.Company;
            entity.Code = this.Code;
            entity.BirthDay = !string.IsNullOrEmpty(this.BirthDay) ? DateTime.Parse(this.BirthDay) : (DateTime?)null;
            entity.TaxCode = this.TaxCode;
            entity.Avatar = this.Avatar;
            entity.PhoneNumber = this.PhoneNumber;
            entity.Email = this.Email;
            entity.Address = this.Address;
            entity.Descreption = this.Descreption;
            entity.ModifyBy = CurrentUser.Instance.User.UserName;
            entity.ModifyDate = DateTime.Now;
            entity.Status = this.Status;
            return entity;
        }
    }
}
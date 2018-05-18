using cvmk.context.IdentityConfiguration;
using hdcontext.AdminDomain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cvmksite.Models.ViewModel
{
    public class CompanyInfoViewmodel
    {
        public int Id { get; set; }
        public string ImageIcon { get; set; }
        public string LongTittle { get; set; }
        public string Name { get; set; }
        public string ShortTittle { get; set; }
        public string SrefLong { get; set; }
        public string SrefShort { get; set; }
        public bool Status { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }

        public CompanyInfo UpdateModel()
        {
            var rs = new CompanyInfo()
            {
                Name = this.Name,
                LongTittle = this.LongTittle,
                Status = this.Status,
                CreateBy = CurrentUser.Instance.User.UserName,
                CreateDate = DateTime.Now
            };

            return rs;
        }

        public CompanyInfo UpdateModel(CompanyInfo entity)
        {
            entity.LongTittle = this.LongTittle;
            entity.Name = this.Name;
            entity.ModifyBy = CurrentUser.Instance.User.UserName;
            entity.ModifyDate = DateTime.Now;
        
            return entity;
        }
    }
}
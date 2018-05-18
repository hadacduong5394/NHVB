using cvmk.context.domain;
using cvmk.context.IdentityConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cvmksite.Models.ViewModel
{
    public class TypeProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public TypeProductCategory UpdateModel()
        {
            return new TypeProductCategory
            {
                Name = this.Name,
                Status = this.Status,
                CreateBy = CurrentUser.Instance.User.UserName,
                CreateDate = DateTime.Now,
                ComId = CurrentUser.Instance.User.ComId
            };
        }

        public TypeProductCategory UpdateModel(TypeProductCategory entity)
        {
            entity.Name = this.Name;
            entity.Status = this.Status;
            entity.ModifyBy = CurrentUser.Instance.User.UserName;
            entity.ModifyDate = DateTime.Now;
            return entity;
        }
    }
}
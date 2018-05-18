using cvmk.context.domain.Material;
using cvmk.context.IdentityConfiguration;
using cvmk.service.Interface;
using hdcore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cvmksite.Models.ViewModel
{
    public class MaterialViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Descreption { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public int RootPrice { get; set; }
        public string Unit { get; set; }
        public int ComId { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public bool Status { get; set; }

        public Material UpdateModel()
        {
            return new Material
            {
                Id = this.Id,
                Code = this.Code,
                ComId = CurrentUser.Instance.User.ComId,
                CreateBy = CurrentUser.Instance.User.UserName,
                CreateDate = DateTime.Now,
                Descreption = this.Descreption,
                Image = this.Image,
                Name = this.Name,
                Quantity = this.Quantity,
                RootPrice = this.RootPrice,
                Unit = this.Unit,
                Status = this.Status
            };
        }

        public Material UpdateModel(int material_id)
        {
            var entity = IoC.Resolve<IMaterialService>().GetbyKey(material_id);
            entity.Code = this.Code;
            entity.Descreption = this.Descreption;
            entity.Name = this.Name;
            entity.Quantity = this.Quantity;
            entity.RootPrice = this.RootPrice;
            entity.Unit = this.Unit;
            entity.Status = this.Status;
            entity.ModifyBy = CurrentUser.Instance.User.UserName;
            entity.ModifyDate = DateTime.Now;
            entity.Image = this.Image;
            return entity;
        }
    }
}
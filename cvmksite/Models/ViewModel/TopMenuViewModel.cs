using cvmk.context.IdentityConfiguration;
using cvmk.service.Interface;
using hdcontext.AdminDomain.Domain;
using hdcore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cvmksite.Models.ViewModel
{
    public class TopMenuViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrderNumber { get; set; }
        public int? ParentId { get; set; }
        public string UI_SREF { get; set; }
        public string ICon { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public bool Status { get; set; }
        public IList<TopMenuViewModel> Childs
        {
            get
            {
                var rs = new List<TopMenuViewModel>();
                if (!this.ParentId.HasValue)
                {
                    var topSrv = IoC.Resolve<ITopMenuService>();
                    rs = topSrv.GetChilds(this.Id).Select(m => new TopMenuViewModel
                    {
                        Id = m.Id,
                        Name = m.Name,
                        OrderNumber = m.OrderNumber,
                        ParentId = m.ParentId,
                        UI_SREF = m.UI_SREF,
                        ICon = m.Icon
                    }).ToList();
                }
                return rs;
            }
        }

        public TopMenu UpdateModel()
        {
            return new TopMenu
            {
                Id = this.Id,
                Name = this.Name,
                OrderNumber = this.OrderNumber,
                Icon = this.ICon,
                ParentId = this.ParentId,
                Status = this.Status,
                UI_SREF = this.UI_SREF,
                CreateBy = CurrentUser.Instance.User.UserName,
                CreateDate = DateTime.Now
            };
        }

        public TopMenu UpdateModel(TopMenu entity)
        {
            entity.Name = this.Name;
            entity.OrderNumber = this.OrderNumber;
            entity.ParentId = this.ParentId;
            entity.UI_SREF = this.UI_SREF;
            entity.Icon = this.ICon;
            entity.Status = this.Status;
            entity.ModifyBy = CurrentUser.Instance.User.UserName;
            entity.ModifyDate = DateTime.Now;
            return entity;
        }
    }
}
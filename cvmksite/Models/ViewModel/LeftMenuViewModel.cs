using cvmk.context.IdentityConfiguration;
using cvmk.service.Interface;
using hdcontext.AdminDomain.Domain;
using hdcore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cvmksite.Models.ViewModel
{
    public class LeftMenuViewModel
    {
        public string Icon { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrderNumber { get; set; }
        public int? ParentId { get; set; }
        public string UI_SREF { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public bool Status { get; set; }

        public IList<LeftMenuViewModel> Childs
        {
            get
            {
                var rs = new List<LeftMenuViewModel>();
                if (!this.ParentId.HasValue)
                {
                    var leftSrv = IoC.Resolve<ILeftMenuService>();
                    rs = leftSrv.GetChilds(this.Id).Select(n => new LeftMenuViewModel
                    {
                        Id = n.Id,
                        Icon = n.Icon,
                        Name = n.Name,
                        OrderNumber = n.OrderNumber,
                        ParentId = n.ParentId,
                        UI_SREF = n.UI_SREF,
                        CreateBy = n.CreateBy,
                        CreateDate = n.CreateDate.ToString("dd/MM/yyyy"),
                        ModifyBy = n.ModifyBy,
                        ModifyDate = n.ModifyDate.HasValue ? n.ModifyDate.Value.ToString("dd/MM/yyyy") : string.Empty,
                        Status = n.Status
                    }).ToList();
                }
                return rs;
            }
        }

        public bool IsParent { get { return this.ParentId.HasValue; } }

        public LeftMenu UpdateModel()
        {
            var lm = new LeftMenu();
            lm.Id = this.Id;
            lm.Icon = this.Icon;
            lm.Name = this.Name;
            lm.OrderNumber = this.OrderNumber;
            lm.ParentId = this.ParentId;
            lm.UI_SREF = this.UI_SREF;
            lm.CreateBy = CurrentUser.Instance.User.UserName;
            lm.CreateDate = DateTime.Now;
            lm.Status = this.Status;
            return lm;
        }

        public LeftMenu UpdateModel(LeftMenu entity)
        {
            entity.Name = this.Name;
            entity.Icon = this.Icon;
            entity.ParentId = this.ParentId;
            entity.OrderNumber = this.OrderNumber;
            entity.Status = this.Status;
            entity.UI_SREF = this.UI_SREF;
            entity.ModifyBy = CurrentUser.Instance.User.UserName;
            entity.ModifyDate = DateTime.Now;
            return entity;
        }
    }
}
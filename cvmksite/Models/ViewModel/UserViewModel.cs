using cvmk.context.IdentityConfiguration;
using cvmk.service.Interface;
using hdcontext.IdentityDomain;
using hdcore;
using hdidentity.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cvmksite.Models.ViewModel
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Address { get; set; }
        public string BirthDay { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; } = true;
        public string PhoneNumber { get; set; }
        public bool Status { get; set; }
        public int TeamId { get; set; } = 1;
        public int ComId { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateDate { get; set; }
        public string InputGroupId { get; set; }
        public IList<Group> Groups
        {
            get
            {
                var groupSrv = IoC.Resolve<IGroupService>();
                var ugSrv = IoC.Resolve<IUserGroupService>();
                var lstgId = ugSrv.GetMulti(n => n.UserId == this.Id).Select(n => n.GroupId);
                return groupSrv.GetMulti(p => lstgId.Contains(p.Id)).ToList();
            }
        }
        public string ComName
        {
            get
            {
                if (this.ComId == -1)
                {
                    return "Root";
                }
                else if (this.ComId == 0)
                {
                    return "System Admin";
                }
                return IoC.Resolve<ICompanyInfoService>().GetbyKey(this.ComId).Name;
            }
        }

        public ApplicationUser UpdateModel()
        {
            var model = new ApplicationUser()
            {
                Id = !string.IsNullOrEmpty(this.Id) ? this.Id : Guid.NewGuid().ToString("N"),
                UserName = this.UserName,
                FullName = this.FullName,
                Address = this.Address,
                Email = this.Email,
                EmailConfirmed = this.EmailConfirmed,
                PhoneNumber = this.PhoneNumber,
                BirthDay = !string.IsNullOrEmpty(this.BirthDay) ? DateTime.Parse(this.BirthDay) : (DateTime?)null,
                CreateBy = !string.IsNullOrEmpty(this.CreateBy) ? this.CreateBy : CurrentUser.Instance.User.UserName,
                CreateDate = !string.IsNullOrEmpty(this.CreateDate) ? DateTime.Parse(this.CreateDate) : DateTime.Now,
                UpdateBy = !string.IsNullOrEmpty(this.UpdateBy) ? this.UpdateBy : string.Empty,
                UpdateDate = !string.IsNullOrEmpty(this.UpdateDate) ? DateTime.Parse(this.UpdateDate) : (DateTime?)null,
                TeamId = this.TeamId,
                ComId = GetComId(),
                Status = this.Status
            };
            return model;
        }

        private int GetComId()
        {
            if (this.TeamId == 0 && CurrentUser.Instance.User.ComId == -1)
            {
                return -1;
            }
            else if (CurrentUser.Instance.User.ComId == -1 && TeamId == 1)
            {
                return this.ComId;
            }
            else
            {
                return CurrentUser.Instance.User.ComId;
            }
        }
    }
}
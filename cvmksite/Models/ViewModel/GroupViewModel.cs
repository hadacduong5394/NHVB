using cvmk.context.IdentityConfiguration;
using hdcontext.IdentityDomain;
using hdcore;
using hdidentity.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace cvmksite.Models.ViewModel
{
    public class GroupViewModel
    {
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string Descreption { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateDate { get; set; }
        public string InputRoles { get; set; }
        public bool IsCheck { get; set; }

        public IList<Role> Roles
        {
            get
            {
                return IoC.Resolve<IGroupService>().GetRolesOfGroup(this.Id);
            }
        }

        public IList<int> DeserilizeRole()
        {
            return JsonConvert.DeserializeObject<List<int>>(this.InputRoles);
        }

        public Group UpdateModel()
        {
            var group = new Group();

            group.Id = this.Id;
            group.Name = this.Name;
            group.Descreption = this.Descreption;
            group.Status = this.Status;
            group.CreateBy = this.CreateBy;
            group.UpdateBy = this.UpdateBy;
            group.ComId = CurrentUser.Instance.User.ComId;
            group.CreateDate = !string.IsNullOrEmpty(this.CreateDate) ? DateTime.Parse(this.CreateDate) : DateTime.Now;
            return group;
        }
    }
}
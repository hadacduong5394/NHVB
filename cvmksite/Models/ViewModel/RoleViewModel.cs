using hdcontext.IdentityDomain;
using System;

namespace cvmksite.Models.ViewModel
{
    public class RoleViewModel
    {
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string Descreption { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateDate { get; set; }
        public bool IsCheck { get; set; }

        public Role UpdateModel()
        {
            return new Role
            {
                Id = this.Id,
                Name = this.Name,
                Descreption = this.Descreption,
                Status = this.Status,
                CreateBy = this.CreateBy,
                CreateDate = !string.IsNullOrEmpty(this.CreateDate) ? DateTime.Parse(this.CreateDate) : DateTime.Now,
                UpdateBy = this.UpdateBy
            };
        }
    }
}
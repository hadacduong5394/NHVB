using cvmk.context.domain;
using cvmk.context.IdentityConfiguration;
using cvmk.service.Interface;
using hdcore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cvmksite.Models.ViewModel
{
    public class FloorViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Descreption { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public bool Status { get; set; }
        public bool VIP { get; set; }
        public IList<Room> Rooms
        {
            get
            {
                var rs = new List<Room>();
                var roomSrv = IoC.Resolve<IRoomService>();
                rs.AddRange(roomSrv.GetMulti(n => n.FloorId == this.Id));
                return rs;
            }
        }

        //update view model to model when create
        public Floor UpdateModel()
        {
            return new Floor
            {
                Id = this.Id,
                Name = this.Name,
                Descreption = this.Descreption,
                Status = this.Status,
                VIP = this.VIP,
                CreateBy = CurrentUser.Instance.User.UserName,
                CreateDate = DateTime.Now,
            };
        }

        //update view model to model to edit
        public Floor UpdateModel(Floor floor)
        {
            floor.Name = this.Name;
            floor.Descreption = this.Descreption;
            floor.Status = this.Status;
            floor.VIP = this.VIP;
            return floor;
        }
    }
}
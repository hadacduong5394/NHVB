using cvmk.context.domain;
using cvmk.context.IdentityConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cvmksite.Models.ViewModel
{
    public class RoomViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Descreption { get; set; }
        public int FloorId { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public bool Status { get; set; }
        public bool IsWorking { get; set; }

        //update view model to model when create
        public Room UpdateModel()
        {
            return new Room
            {
                Id = this.Id,
                Name = this.Name,
                FloorId = this.FloorId,
                Descreption = this.Descreption,
                Status = this.Status,
                IsWorking = this.IsWorking,
                CreateBy = CurrentUser.Instance.User.UserName,
                CreateDate = DateTime.Now,
            };
        }

        //update view model to model to edit
        public Room UpdateModel(Room room)
        {
            room.Name = this.Name;
            room.Descreption = this.Descreption;
            room.Status = this.Status;
            room.IsWorking = this.IsWorking;
            room.FloorId = this.FloorId;
            return room;
        }
    }
}
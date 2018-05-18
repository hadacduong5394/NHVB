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
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int TableId { get; set; }
        public string TableName { get; set; }
        public bool TableStatus
        {
            get
            {
                return TableId != 0 ? IoC.Resolve<IRoomService>().GetbyKey(this.TableId).IsWorking : false;
            }
        }
        public string Descreption { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string CreateDateString { get; set; }
        public decimal Sale { get; set; }
        public decimal Payed { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Total { get; set; }// = totalamount - sale - payed
        public bool Status { get; set; } // true = trả đủ, false = trả 1 phần
        public bool IsPayment { get; set; }
        public string JsonOrderDetail { get; set; }
        public Order UpdateModel()
        {
            return new Order
            {
                Id = this.Id,
                Code = this.Code,
                EmployeeId = CurrentUser.Instance.User.Id,
                EmployeeName = CurrentUser.Instance.User.FullName,
                CustomerName = this.CustomerName,
                CustomerPhone = this.CustomerPhone,
                CustomerAddress = this.CustomerAddress,
                CustomerEmail = this.CustomerEmail,
                CreateDate = DateTime.Now,
                Payed = this.Payed,
                Sale = this.Sale,
                TotalAmount = this.TotalAmount,
                Total = this.TotalAmount - this.Sale,
                Status = this.Status,
                TableId = this.TableId,
                TableName = this.TableName,
                Descreption = this.Descreption
            };
        }
        public void SerilizableDetail(IList<OrderDetailViewModel> lstDetail)
        {
            this.JsonOrderDetail = Newtonsoft.Json.JsonConvert.SerializeObject(lstDetail);
        }
        public IList<OrderDetailViewModel> DeSerilizableDetail()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<IList<OrderDetailViewModel>>(this.JsonOrderDetail);
        }
    }
}
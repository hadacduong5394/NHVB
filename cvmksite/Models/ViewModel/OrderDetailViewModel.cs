using cvmk.context.domain;
using cvmk.service.Interface;
using hdcore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cvmksite.Models.ViewModel
{
    public class OrderDetailViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal RootPrice { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; } //Amount = Quantity * Price
        public OrderDetail UpdateModel()
        {
            return new OrderDetail
            {
                Id = this.Id,
                ProductId = this.ProductId,
                ProductCode = this.ProductCode,
                ProductName = this.ProductName,
                RootPrice = IoC.Resolve<IProductService>().GetbyKey(this.ProductId).RootPrice,
                Price = this.Price,
                Quantity = this.Quantity,
                Amount = Price * Quantity,
                OrderId = this.OrderId
            };
        }
        public static IList<OrderDetail> UpdateModels(IList<OrderDetailViewModel> lstDetail)
        {
            var result = new List<OrderDetail>();
            foreach (var item in lstDetail)
            {
                result.Add(new OrderDetail {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductCode = item.ProductCode,
                    ProductName = item.ProductName,
                    RootPrice = IoC.Resolve<IProductService>().GetbyKey(item.ProductId).RootPrice,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Amount = item.Price * item.Quantity
                });
            }
            return result;
        }
    }
}
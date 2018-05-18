using cvmk.context.domain;
using cvmk.context.domain.Products;
using cvmk.context.IdentityConfiguration;
using cvmk.service.Interface;
using hdcore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cvmksite.Models.ViewModel
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public int ComId { get; set; }
        public string BarCode { get; set; }
        public string Name { get; set; }
        public string Descreption { get; set; }
        public string Image { get; set; }
        public string Images { get; set; }
        public string Content { get; set; }
        public int Quantity { get; set; }
        public decimal RootPrice { get; set; }
        public decimal Price { get; set; }
        public int TypeId { get; set; }
        public int GroupId { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public bool Status { get; set; }
        public bool VIP { get; set; }
        public string Unit { get; set; }
        public string JsonProps { get; set; }
        public string JsonMaterials { get; set; }
        public GroupProductCategory Group { get { return IoC.Resolve<IGroupProductCategoryService>().GetbyKey(this.GroupId); } }
        public TypeProductCategory Type { get { return IoC.Resolve<ITypeProductCategoryService>().GetbyKey(this.TypeId); } }

        public Product UpdateModel()
        {
            return new Product
            {
                Id = this.Id,
                BarCode = this.BarCode,
                Content = this.Content,
                CreateBy = CurrentUser.Instance.User.UserName,
                CreateDate = DateTime.Now,
                Descreption = this.Descreption,
                GroupId = this.GroupId,
                Image = this.Image,
                Images = this.Images,
                Name = this.Name,
                Quantity = this.Quantity,
                RootPrice = this.RootPrice,
                Price = this.Price,
                TypeId = this.TypeId,
                Status = this.Status,
                VIP = this.VIP,
                Unit = this.Unit,
                ComId = CurrentUser.Instance.User.ComId
            };
        }

        public Product UpdateModel(Product product)
        {
            product.Name = this.Name;
            product.BarCode = this.BarCode;
            product.Quantity = this.Quantity;
            product.RootPrice = this.RootPrice;
            product.Price = this.Price;
            product.Descreption = this.Descreption;
            product.Content = this.Content;
            product.Unit = this.Unit;
            product.Status = this.Status;
            product.VIP = this.VIP;
            product.TypeId = this.TypeId;
            product.GroupId = this.GroupId;
            product.Image = this.Image;
            product.Images = this.Images;
            product.ModifyBy = CurrentUser.Instance.User.UserName;
            product.ModifyDate = DateTime.Now;
            return product;
        }

        public IList<ProductPropertis> DeserializeProductPropertis()
        {
            if (!string.IsNullOrEmpty(this.JsonProps))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductPropertis>>(JsonProps);
            }
            return new List<ProductPropertis>();
        }

        public void SerializeProductPropertis(List<ProductPropertis> lst)
        {
            this.JsonProps = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
        }

        public IList<ProductMaterials> DeserializeProductMaterials()
        {
            if (!string.IsNullOrEmpty(this.JsonMaterials))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductMaterials>>(JsonMaterials);
            }
            return new List<ProductMaterials>();
        }

        public void SerializeProductMaterials(List<ProductMaterials> lst)
        {
            this.JsonMaterials = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
        }

    }
}
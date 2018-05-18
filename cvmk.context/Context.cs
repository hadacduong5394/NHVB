using cvmk.context.domain;
using cvmk.context.domain.Material;
using cvmk.context.domain.Products;
using hdcontext;
using hdcore;
using hddata.UnitOfWork;
using System.Data.Entity;

namespace cvmk.context
{
    public class Context : ContextConnection
    {
        public Context() : base()
        {
            this.Configuration.LazyLoadingEnabled = true;
        }
        public static Context Create()
        {
            return new Context();
        }

        public DbSet<ArticleCategory> ArticleCategories { get; set; }
        public DbSet<TypeProductCategory> TypeProductCategories { get; set; }
        public DbSet<GroupProductCategory> GroupProductCategories { get; set; }
        public DbSet<UnitTag> UnitTags { get; set; }
        public DbSet<UnitProduct> UnitProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<ImportProduct> ImportProducts { get; set; }
        public DbSet<ImportProductDetail> ImportProductDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<ProductMaterials> ProductMaterials { get; set; }
        public DbSet<ProductPropertis> ProductPropertis { get; set; }
    }
}
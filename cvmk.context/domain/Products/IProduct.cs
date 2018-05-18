using hdcontext.AdminDomain.Abtracttions;

namespace cvmk.context.domain.Products
{
    public interface IProduct : IAuditable
    {
        int Id { get; set; }
        string BarCode { get; set; }
        string Name { get; set; }
        string Descreption { get; set; }
        string Content { get; set; }
        int Quantity { get; set; }
        int TypeId { get; set; }
        int GroupId { get; set; }
        string Images { get; set; }
        string Unit { get; set; }
        int ComId { get; set; }
        bool VIP { get; set; }
    }
}
namespace Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = null!;
    public ProductType? ProductType { get; set; }
    public int ProductTypeId { get; set; }
    public ProductBrand? ProductBrand { get; set; }
    public int ProductBrandId { get; set; }

}

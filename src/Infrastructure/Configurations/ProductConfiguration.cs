using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name).HasMaxLength(255).IsRequired();

        builder.Property(e => e.Description).HasMaxLength(255).IsRequired();

        builder.Property(e => e.Price).HasPrecision(18, 5).IsRequired();

        builder.Property(e => e.ImageUrl).IsRequired();

        builder.HasOne(e => e.ProductBrand).WithMany().HasForeignKey(e => e.ProductBrandId);

        builder.HasOne(e => e.ProductType).WithMany().HasForeignKey(e => e.ProductTypeId);

    }
}

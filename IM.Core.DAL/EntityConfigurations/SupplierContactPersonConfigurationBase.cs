using InfraManager.DAL.Suppliers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class SupplierContactPersonConfigurationBase : IEntityTypeConfiguration<SupplierContactPerson>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string SupplierForeignKeyName { get; }
    protected abstract string Schema { get; }
    protected abstract string TableName { get; }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<SupplierContactPerson> entity);

    public void Configure(EntityTypeBuilder<SupplierContactPerson> entity)
    {
        entity.ToTable(TableName, Schema);

        entity.HasKey(x => x.ID).HasName(PrimaryKeyName);

        entity.Property(e => e.Name).IsRequired(true).HasMaxLength(100);
        entity.Property(e => e.Surname).IsRequired(true).HasMaxLength(100);
        entity.Property(e => e.Patronymic).IsRequired(true).HasMaxLength(100);
        entity.Property(e => e.Phone).IsRequired(true).HasMaxLength(100);
        entity.Property(e => e.SecondPhone).IsRequired(true).HasMaxLength(100);
        entity.Property(e => e.Email).IsRequired(true).HasMaxLength(100);
        entity.Property(e => e.PositionID).IsRequired(false);
        entity.Property(e => e.Note).IsRequired(true).HasMaxLength(500);
        entity.Property(e => e.SupplierID).IsRequired(true);

        entity.HasOne(x => x.Supplier)
                .WithMany()
                .HasForeignKey(x => x.SupplierID)
                .HasPrincipalKey(x => x.ID)
                .HasConstraintName(SupplierForeignKeyName);

        entity.HasOne(x => x.Position)
                .WithMany()
                .HasForeignKey(x => x.PositionID)
                .HasPrincipalKey(x => x.IMObjID);

        ConfigureDatabase(entity);
    }
}
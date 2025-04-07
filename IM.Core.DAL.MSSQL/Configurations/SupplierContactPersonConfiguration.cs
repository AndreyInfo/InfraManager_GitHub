using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Suppliers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

public class SupplierContactPersonConfiguration : SupplierContactPersonConfigurationBase
{
    protected override string PrimaryKeyName => "PK_SupplierContactPerson";
    protected override string SupplierForeignKeyName => "FK_SupplierContactPerson_Supplier";
    protected override string Schema => Options.Scheme;
    protected override string TableName => "SupplierContactPerson";

    protected override void ConfigureDatabase(EntityTypeBuilder<SupplierContactPerson> entity)
    {
        entity.Property(e => e.ID).HasColumnName("ID").HasDefaultValueSql("newid()");

        entity.Property(e => e.Name).HasColumnName("Name");
        entity.Property(e => e.Surname).HasColumnName("Surname");
        entity.Property(e => e.Patronymic).HasColumnName("Patronymic");
        entity.Property(e => e.Phone).HasColumnName("Phone");
        entity.Property(e => e.SecondPhone).HasColumnName("SecondPhone");
        entity.Property(e => e.Email).HasColumnName("Email");
        entity.Property(e => e.PositionID).HasColumnName("PositionID");
        entity.Property(e => e.Note).HasColumnName("Note");
        entity.Property(e => e.SupplierID).HasColumnName("SupplierID");

        entity.Property(x => x.RowVersion)
        .IsRequired()
        .HasColumnName("tstamp")
        .HasColumnType("timestamp")
        .IsRowVersion();
    }
}
using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.Suppliers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

public class SupplierContactPersonConfiguration : SupplierContactPersonConfigurationBase
{
    protected override string PrimaryKeyName => "pk_supplier_contact_person";
    protected override string SupplierForeignKeyName => "fk_supplier_contact_person_supplier";
    protected override string Schema => Options.Scheme;
    protected override string TableName => "supplier_contact_person";

    protected override void ConfigureDatabase(EntityTypeBuilder<SupplierContactPerson> entity)
    {
        entity.Property(e => e.ID).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.Surname).HasColumnName("surname");
        entity.Property(e => e.Patronymic).HasColumnName("patronymic");
        entity.Property(e => e.Phone).HasColumnName("phone");
        entity.Property(e => e.SecondPhone).HasColumnName("second_phone");
        entity.Property(e => e.Email).HasColumnName("email");
        entity.Property(e => e.PositionID).HasColumnName("position_id");
        entity.Property(e => e.Note).HasColumnName("note");
        entity.Property(e => e.SupplierID).HasColumnName("supplier_id");

        entity.HasXminRowVersion(x => x.RowVersion);
    }
}
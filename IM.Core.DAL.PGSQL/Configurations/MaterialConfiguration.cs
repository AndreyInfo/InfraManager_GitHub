using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IM.Core.DAL.Postgres;

namespace InfraManager.DAL.Postgres.Configurations;

internal sealed class MaterialConfiguration : MaterialConfigurationBase
{
    protected override string KeyName => "pk_material";

    protected override string StorageLocationFK => "fk_material_storage_location";

    protected override string MaterialModelFK => "fk_material_material_model";

    protected override void ConfigureDataBase(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable("material", Options.Scheme);

        builder.Property(c => c.MaterialID).HasColumnName("material_id");
        builder.Property(c => c.MaterialOperationID).HasColumnName("material_operation_id");
        builder.Property(c => c.MaterialModelID).HasColumnName("material_model_id");
        builder.Property(c => c.Date).HasColumnName("date");
        builder.Property(c => c.Amount).HasColumnName("amount");
        builder.Property(c => c.Cost).HasColumnType("money").HasColumnName("cost");
        builder.Property(c => c.SupplierID).HasColumnName("supplier_id");
        builder.Property(c => c.Document).HasColumnName("document");
        builder.Property(c => c.UseByDate).HasColumnName("use_by_date");
        builder.Property(c => c.ResponsibleUserID).HasColumnName("responsible_user_id");
        builder.Property(c => c.ExecutiveUserID).HasColumnName("executive_user_id");
        builder.Property(c => c.RoomID).HasColumnName("room_id");
        builder.Property(c => c.PreviousRoomID).HasColumnName("previous_room_id");
        builder.Property(c => c.DeviceID).HasColumnName("device_id");
        builder.Property(c => c.Device).HasColumnName("device");
        builder.Property(c => c.Note).HasColumnName("note");
        builder.Property(c => c.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
        builder.Property(c => c.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(c => c.LifeCycleStateID).HasColumnName("life_cycle_state_id");
        builder.Property(c => c.GoodsInvoiceID).HasColumnName("goods_invoice_id");
        builder.Property(c => c.OwnerID).HasColumnName("owner_id");
        builder.Property(c => c.OwnerClassID).HasColumnName("owner_class_id");
        builder.Property(c => c.StorageLocationID).HasColumnName("storage_location_id");
    }
}

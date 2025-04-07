using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class MaterialConfiguration : MaterialConfigurationBase
{
    protected override string KeyName => "PK_Material";

    protected override string StorageLocationFK => "FK_Material_StorageLocation";

    protected override string MaterialModelFK => "FK_Material_MaterialModel";

    protected override void ConfigureDataBase(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable("Material", Options.Scheme);

        builder.Property(c => c.MaterialID).HasColumnName("MaterialID");
        builder.Property(c => c.MaterialOperationID).HasColumnName("MaterialOperationID");
        builder.Property(c => c.MaterialModelID).HasColumnName("MaterialModelID");
        builder.Property(c => c.Date).HasColumnName("Date");
        builder.Property(c => c.Amount).HasColumnName("Amount");
        builder.Property(c => c.Cost).HasColumnType("money").HasColumnName("Cost");
        builder.Property(c => c.SupplierID).HasColumnName("SupplierID");
        builder.Property(c => c.Document).HasColumnName("Document");
        builder.Property(c => c.UseByDate).HasColumnName("UseByDate");
        builder.Property(c => c.ResponsibleUserID).HasColumnName("ResponsibleUserID");
        builder.Property(c => c.ExecutiveUserID).HasColumnName("ExecutiveUserID");
        builder.Property(c => c.RoomID).HasColumnName("RoomID");
        builder.Property(c => c.PreviousRoomID).HasColumnName("PreviousRoomID");
        builder.Property(c => c.DeviceID).HasColumnName("DeviceID");
        builder.Property(c => c.Device).HasColumnName("Device");
        builder.Property(c => c.Note).HasColumnName("Note");
        builder.Property(c => c.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
        builder.Property(c => c.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(c => c.LifeCycleStateID).HasColumnName("LifeCycleStateID");
        builder.Property(c => c.GoodsInvoiceID).HasColumnName("GoodsInvoiceID");
        builder.Property(c => c.OwnerID).HasColumnName("OwnerID");
        builder.Property(c => c.OwnerClassID).HasColumnName("OwnerClassID");
        builder.Property(c => c.StorageLocationID).HasColumnName("StorageLocationID");
    }
}

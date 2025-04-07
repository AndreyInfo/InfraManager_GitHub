using InfraManager.DAL.ITAsset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class ITAssetUndisposedConfigurationBase : IEntityTypeConfiguration<ITAssetUndisposed>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string FKITAssetUndisposedITAssetImportSetting { get; }

    public void Configure(EntityTypeBuilder<ITAssetUndisposed> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.Name).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.InvNumber).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.SerialNumber).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.Code).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.ExternalID).IsRequired(false).HasMaxLength(250);
        builder.Property(x => x.AssetTag).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.Note).IsRequired(false).HasMaxLength(1000);
        builder.Property(x => x.Description).IsRequired(false).HasMaxLength(250);
        builder.Property(x => x.TypeExternalID).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.TypeName).IsRequired(false).HasMaxLength(500);
        builder.Property(x => x.ModelExternalID).IsRequired(false).HasMaxLength(250);
        builder.Property(x => x.ModelName).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.VendorExternalID).IsRequired(false).HasMaxLength(250);
        builder.Property(x => x.VendorName).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.SupplierExternalID).IsRequired(false).HasMaxLength(250);
        builder.Property(x => x.SupplierName).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.OwnerExternalID).IsRequired(false).HasMaxLength(500);
        builder.Property(x => x.OwnerName).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.UtilizerExternalID).IsRequired(false).HasMaxLength(500);
        builder.Property(x => x.UtilizerName).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.EquipmentExternalID).IsRequired(false).HasMaxLength(250);
        builder.Property(x => x.EquipmentName).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.RackExternalID).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.RackName).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.WorkplaceExternalID).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.WorkplaceName).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.RoomExternalID).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.RoomName).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.Location).IsRequired(false).HasMaxLength(500);
        builder.Property(x => x.BuildingExternalID).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.BuildingName).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.Agreement).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.UserName).IsRequired(false).HasMaxLength(100);
        builder.Property(x => x.Founding).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.UserField1).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.UserField2).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.UserField3).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.UserField4).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.UserField5).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.IpAddress).IsRequired(false).HasMaxLength(15);
        builder.Property(x => x.MacAddress).IsRequired(false).HasMaxLength(23);
        builder.Property(x => x.Domain).IsRequired(false).HasMaxLength(100);
        builder.Property(x => x.DomainLogin).IsRequired(false).HasMaxLength(100);
        builder.Property(x => x.Login).IsRequired(false).HasMaxLength(100);
        builder.Property(x => x.OS).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.IpMask).IsRequired(false).HasMaxLength(15);

        builder.HasOne(x => x.ITAssetImportSetting)
        .WithMany()
        .HasForeignKey(x => x.ITAssetImportSettingID)
        .HasConstraintName(FKITAssetUndisposedITAssetImportSetting)
        .OnDelete(DeleteBehavior.Cascade);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<ITAssetUndisposed> builder);
}

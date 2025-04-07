using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ITAsset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;
public class ITAssetUndisposedConfiguration : ITAssetUndisposedConfigurationBase
{
    protected override string PrimaryKeyName => "PK_ITAssetUndisposed";
    protected override string FKITAssetUndisposedITAssetImportSetting => "FK_ITAssetUndisposed_ITAssetImportSetting";

    protected override void ConfigureDataBase(EntityTypeBuilder<ITAssetUndisposed> builder)
    {
        builder.ToTable("ITAssetUndisposed", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.ReasonCode).HasColumnName("ReasonCode");
        builder.Property(x => x.ITAssetImportSettingID).HasColumnName("ITAssetImportSettingID");
        builder.Property(x => x.DetectionTime).HasColumnName("DetectionTime");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.InvNumber).HasColumnName("InvNumber");
        builder.Property(x => x.SerialNumber).HasColumnName("SerialNumber");
        builder.Property(x => x.Code).HasColumnName("Code");
        builder.Property(x => x.ExternalID).HasColumnName("ExternalID");
        builder.Property(x => x.AssetTag).HasColumnName("AssetTag");
        builder.Property(x => x.Note).HasColumnName("Note");
        builder.Property(x => x.Description).HasColumnName("Description");
        builder.Property(x => x.TypeExternalID).HasColumnName("TypeExternalID");
        builder.Property(x => x.TypeName).HasColumnName("TypeName");
        builder.Property(x => x.ModelExternalID).HasColumnName("ModelExternalID");
        builder.Property(x => x.ModelName).HasColumnName("ModelName");
        builder.Property(x => x.VendorExternalID).HasColumnName("VendorExternalID");
        builder.Property(x => x.VendorName).HasColumnName("VendorName");
        builder.Property(x => x.SupplierExternalID).HasColumnName("SupplierExternalID");
        builder.Property(x => x.SupplierName).HasColumnName("SupplierName");
        builder.Property(x => x.OwnerExternalID).HasColumnName("OwnerExternalID");
        builder.Property(x => x.OwnerName).HasColumnName("OwnerName");
        builder.Property(x => x.UtilizerExternalID).HasColumnName("UtilizerExternalID");
        builder.Property(x => x.UtilizerName).HasColumnName("UtilizerName");
        builder.Property(x => x.EquipmentExternalID).HasColumnName("EquipmentExternalID");
        builder.Property(x => x.EquipmentName).HasColumnName("EquipmentName");
        builder.Property(x => x.RackExternalID).HasColumnName("RackExternalID");
        builder.Property(x => x.RackName).HasColumnName("RackName");
        builder.Property(x => x.WorkplaceExternalID).HasColumnName("WorkplaceExternalID");
        builder.Property(x => x.WorkplaceName).HasColumnName("WorkplaceName");
        builder.Property(x => x.RoomExternalID).HasColumnName("RoomExternalID");
        builder.Property(x => x.RoomName).HasColumnName("RoomName");
        builder.Property(x => x.Location).HasColumnName("Location");
        builder.Property(x => x.BuildingExternalID).HasColumnName("BuildingExternalID");
        builder.Property(x => x.BuildingName).HasColumnName("BuildingName");
        builder.Property(x => x.Agreement).HasColumnName("Agreement");
        builder.Property(x => x.UserName).HasColumnName("UserName");
        builder.Property(x => x.Founding).HasColumnName("Founding");
        builder.Property(x => x.Cost).HasColumnName("Cost");
        builder.Property(x => x.Warranty).HasColumnName("Warranty");
        builder.Property(x => x.DateReceived).HasColumnName("DateReceived");
        builder.Property(x => x.UserField1).HasColumnName("UserField1");
        builder.Property(x => x.UserField2).HasColumnName("UserField2");
        builder.Property(x => x.UserField3).HasColumnName("UserField3");
        builder.Property(x => x.UserField4).HasColumnName("UserField4");
        builder.Property(x => x.UserField5).HasColumnName("UserField5");
        builder.Property(x => x.IpAddress).HasColumnName("IpAddress");
        builder.Property(x => x.MacAddress).HasColumnName("MacAddress");
        builder.Property(x => x.Domain).HasColumnName("Domain");
        builder.Property(x => x.DomainLogin).HasColumnName("DomainLogin");
        builder.Property(x => x.Login).HasColumnName("Login");
        builder.Property(x => x.OS).HasColumnName("OS");
        builder.Property(x => x.IpMask).HasColumnName("IpMask");
    }
}

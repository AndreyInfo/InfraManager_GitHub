using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ITAsset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;
public class ITAssetUndisposedConfiguration : ITAssetUndisposedConfigurationBase
{
    protected override string PrimaryKeyName => "pk_it_asset_undisposed";
    protected override string FKITAssetUndisposedITAssetImportSetting => "fk_it_asset_undisposed_it_asset_import_setting";

    protected override void ConfigureDataBase(EntityTypeBuilder<ITAssetUndisposed> builder)
    {
        builder.ToTable("it_asset_undisposed", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.ReasonCode).HasColumnName("reason_code");
        builder.Property(x => x.ITAssetImportSettingID).HasColumnName("it_asset_import_setting_id");
        builder.Property(x => x.DetectionTime).HasColumnName("detection_time");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.InvNumber).HasColumnName("inv_number");
        builder.Property(x => x.SerialNumber).HasColumnName("serial_number");
        builder.Property(x => x.Code).HasColumnName("code");
        builder.Property(x => x.ExternalID).HasColumnName("external_id");
        builder.Property(x => x.AssetTag).HasColumnName("asset_tag");
        builder.Property(x => x.Note).HasColumnName("note");
        builder.Property(x => x.Description).HasColumnName("description");
        builder.Property(x => x.TypeExternalID).HasColumnName("type_external_id");
        builder.Property(x => x.TypeName).HasColumnName("type_name");
        builder.Property(x => x.ModelExternalID).HasColumnName("model_external_id");
        builder.Property(x => x.ModelName).HasColumnName("model_name");
        builder.Property(x => x.VendorExternalID).HasColumnName("vendor_external_id");
        builder.Property(x => x.VendorName).HasColumnName("vendor_name");
        builder.Property(x => x.SupplierExternalID).HasColumnName("supplier_external_id");
        builder.Property(x => x.SupplierName).HasColumnName("supplier_name");
        builder.Property(x => x.OwnerExternalID).HasColumnName("owner_external_id");
        builder.Property(x => x.OwnerName).HasColumnName("owner_name");
        builder.Property(x => x.UtilizerExternalID).HasColumnName("utilizer_external_id");
        builder.Property(x => x.UtilizerName).HasColumnName("utilizer_name");
        builder.Property(x => x.EquipmentExternalID).HasColumnName("equipment_external_id");
        builder.Property(x => x.EquipmentName).HasColumnName("equipment_name");
        builder.Property(x => x.RackExternalID).HasColumnName("rack_external_id");
        builder.Property(x => x.RackName).HasColumnName("rack_name");
        builder.Property(x => x.WorkplaceExternalID).HasColumnName("workplace_external_id");
        builder.Property(x => x.WorkplaceName).HasColumnName("workplace_name");
        builder.Property(x => x.RoomExternalID).HasColumnName("room_external_id");
        builder.Property(x => x.RoomName).HasColumnName("room_name");
        builder.Property(x => x.Location).HasColumnName("location");
        builder.Property(x => x.BuildingExternalID).HasColumnName("building_external_id");
        builder.Property(x => x.BuildingName).HasColumnName("building_name");
        builder.Property(x => x.Agreement).HasColumnName("agreement");
        builder.Property(x => x.UserName).HasColumnName("user_name");
        builder.Property(x => x.Founding).HasColumnName("founding");
        builder.Property(x => x.Cost).HasColumnName("cost");
        builder.Property(x => x.Warranty).HasColumnName("warranty");
        builder.Property(x => x.DateReceived).HasColumnName("date_received");
        builder.Property(x => x.UserField1).HasColumnName("user_field1");
        builder.Property(x => x.UserField2).HasColumnName("user_field2");
        builder.Property(x => x.UserField3).HasColumnName("user_field3");
        builder.Property(x => x.UserField4).HasColumnName("user_field4");
        builder.Property(x => x.UserField5).HasColumnName("user_field5");
        builder.Property(x => x.IpAddress).HasColumnName("ip_address");
        builder.Property(x => x.MacAddress).HasColumnName("mac_address");
        builder.Property(x => x.Domain).HasColumnName("domain");
        builder.Property(x => x.DomainLogin).HasColumnName("domain_login");
        builder.Property(x => x.Login).HasColumnName("login");
        builder.Property(x => x.OS).HasColumnName("os");
        builder.Property(x => x.IpMask).HasColumnName("ip_mask");
    }
}

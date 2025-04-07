using InfraManager.DAL.Software;
using IM.Core.DAL.PGSQL;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using IM.Core.DAL.Postgres;


namespace IM.Core.DAL.PGSQL.Configurations
{
    public partial class UndisposedDeviceConfiguration : IEntityTypeConfiguration<UndisposedDevice>
    {
        public void Configure(EntityTypeBuilder<UndisposedDevice> entity)
        {
            entity.ToTable("undisposed_device", Options.Scheme);
            entity.Property(e => e.IsLogical).HasColumnName("is_logical");
            entity.Property(e => e.ProviderType).HasColumnName("provider_type");



            entity.HasKey(e => e.DeviceId)
                .HasName("pk__undisposed_device");

            //entity.HasIndex(e => e.DeviceId, "ix_undisposed_device_device_id");

            //entity.HasIndex(e => e.IMObjID, "ix_undisposed_device_im_obj_id");

            entity.Property(e => e.DeviceId)
                .ValueGeneratedNever()
                .HasColumnName("device_id");

            entity.Property(e => e.Agreement).HasColumnName("agreement")
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.Property(e => e.BiosName).HasColumnName("bios_name").HasMaxLength(255);

            entity.Property(e => e.BiosVersion).HasColumnName("bios_version").HasMaxLength(255);

            entity.Property(e => e.BuildingName).HasColumnName("building_name").HasMaxLength(255);

            entity.Property(e => e.ClassId).HasColumnName("class_id");

            entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(50);

            entity.Property(e => e.ComputerName).HasColumnName("computer_name").HasMaxLength(255);

            entity.Property(e => e.Cost).HasColumnName("cost").HasColumnType("decimal(10, 2)");

            entity.Property(e => e.CsFormFactor).HasColumnName("cs_form_factor").HasMaxLength(255);

            entity.Property(e => e.CsModel).HasColumnName("cs_model").HasMaxLength(255);

            entity.Property(e => e.CsSize).HasColumnName("cs_size").HasMaxLength(255);

            entity.Property(e => e.CsVendorId).HasColumnName("cs_vendor_id");

            entity.Property(e => e.DateInquiry).HasColumnName("date_inquiry").HasColumnType("datetime");

            entity.Property(e => e.DateReceived).HasColumnName("date_received").HasColumnType("datetime");

            entity.Property(e => e.DefaultPrinter).HasColumnName("default_printer").HasMaxLength(255);

            entity.Property(e => e.Description).HasColumnName("description")
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.Property(e => e.ExternalId)
                .HasMaxLength(250)
                .HasColumnName("external_id");

            entity.Property(e => e.Founding).HasColumnName("founding")
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.Property(e => e.IMObjID).HasColumnName("im_obj_id");

            entity.Property(e => e.InvNumber).HasColumnName("inv_number").HasMaxLength(50);

            entity.Property(e => e.Ipaddress)
                .HasMaxLength(255)
                .HasColumnName("ip_address");

            entity.Property(e => e.LocationString).HasColumnName("location_string").HasMaxLength(2000);

            entity.Property(e => e.LogonDomain).HasColumnName("logon_domain")
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Macaddress)
                .HasMaxLength(255)
                .HasColumnName("mac_address");

            entity.Property(e => e.ManufacturerName).HasColumnName("manufacturer_name")
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.Property(e => e.ModelExternalId)
                .HasMaxLength(250)
                .HasColumnName("model_external_id");

            entity.Property(e => e.ModelName).HasColumnName("model_name")
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.Property(e => e.Note).HasColumnName("note").HasMaxLength(255);

            entity.Property(e => e.OwningOrganization).HasColumnName("owning_organization")
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.Property(e => e.ProductCatalogTemplateId).HasColumnName("product_catalog_template_id");

            entity.Property(e => e.ReportId).HasColumnName("report_id");

            entity.Property(e => e.ResponsibleUser).HasColumnName("responsible_user")
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.Property(e => e.RoomName).HasColumnName("room_name").HasMaxLength(255);

            entity.Property(e => e.SerlNumber).HasColumnName("serl_number").HasMaxLength(50);

            entity.Property(e => e.SmbiosassetTag).HasColumnName("smbios_asset_tag")
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.SourceName).HasColumnName("source_name").HasMaxLength(500);

            entity.Property(e => e.SubNetMask).HasColumnName("sub_net_mask").HasMaxLength(255);

            entity.Property(e => e.SupplierExternalId)
                .HasMaxLength(250)
                .HasColumnName("supplier_external_id");

            entity.Property(e => e.SupplierName).HasColumnName("supplier_name")
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.Property(e => e.TypeName).HasColumnName("type_name").HasMaxLength(250);

            entity.Property(e => e.UserField1).HasColumnName("user_field1").HasMaxLength(255);

            entity.Property(e => e.UserField2).HasColumnName("user_field2").HasMaxLength(255);

            entity.Property(e => e.UserField3).HasColumnName("user_field3").HasMaxLength(255);

            entity.Property(e => e.UserField4).HasColumnName("user_field4").HasMaxLength(255);

            entity.Property(e => e.UserField5).HasColumnName("user_field5").HasMaxLength(255);

            entity.Property(e => e.UserName).HasColumnName("user_name")
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.UtilizerName).HasColumnName("utilizer_name")
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.Property(e => e.Warranty).HasColumnName("warranty").HasColumnType("timestamp(3)");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<UndisposedDevice> entity);
    }
}

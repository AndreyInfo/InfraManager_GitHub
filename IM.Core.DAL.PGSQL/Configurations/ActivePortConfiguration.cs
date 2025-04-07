using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IM.Core.DAL.PGSQL.Configurations
{
    public partial class ActivePortConfiguration : ActivePortConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_active_port";
        protected override string Schema => Options.Scheme;
        protected override string TableName => "active_port";
        protected override string IDDefaultValue => "nextval('active_port_id_seq')";
        protected override string IMObjIDDefaultValue => "im.gen_random_uuid()";
        protected override string IMObjIDIndexName => "ix_active_port_im_obj_id";
        protected override string OutletPanelIDIndexName => "ix_active_port_outlet_panel_id";

        protected override void AdditionalConfig(EntityTypeBuilder<ActivePort> entity)
        {
            entity.Property(e => e.ID).HasColumnName("identificator");
            entity.Property(e => e.PortName).HasColumnName("name");
            entity.Property(e => e.JackTypeID).HasColumnName("jack_type_id");
            entity.Property(e => e.TechnologyTypeID).HasColumnName("technology_id");
            entity.Property(e => e.PortAddress).HasColumnName("mac_address");
            entity.Property(e => e.PortIPX).HasColumnName("ipx_net");
            entity.Property(e => e.GroupNumber).HasColumnName("group_number");
            entity.Property(e => e.PortSpeed).HasColumnName("speed");
            entity.Property(e => e.PortVLAN).HasColumnName("vlan_number");
            entity.Property(e => e.PortFilter).HasColumnName("filter");
            entity.Property(e => e.PortState).HasColumnName("state");
            entity.Property(e => e.PortStatus).HasColumnName("port_status_id");
            entity.Property(e => e.PortModule).HasColumnName("port_module");
            entity.Property(e => e.SlotNumber).HasColumnName("slot_number");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.ActiveEquipmentID).HasColumnName("active_equipment_id");
            entity.Property(e => e.PortNumber).HasColumnName("port_number");
            entity.Property(e => e.Connected).HasColumnName("connected");
            entity.Property(e => e.Connection1).HasColumnName("connection1");
            entity.Property(e => e.VisioID).HasColumnName("visio_id");
            entity.Property(e => e.ExternalID).HasColumnName("external_id");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.ClassID).HasColumnName("class_id");
            entity.Property(e => e.TelephoneLineTypeID).HasColumnName("telephone_line_type_id");
            entity.Property(e => e.TelephoneNumber).HasColumnName("telephone_number");
            entity.Property(e => e.TelephoneCategoryID).HasColumnName("telephone_category_id");
            entity.Property(e => e.VoiceMail).HasColumnName("voice_mail");
            entity.Property(e => e.RingGroup).HasColumnName("ring_group");
            entity.Property(e => e.PickUpGroup).HasColumnName("pick_up_group");
            entity.Property(e => e.HuntingGroup).HasColumnName("hunting_group");
            entity.Property(e => e.PermisionGroup).HasColumnName("permision_group");
            entity.Property(e => e.PageGroup).HasColumnName("page_group");
            entity.Property(e => e.Connection).HasColumnName("connection");
            entity.Property(e => e.ConnectorID).HasColumnName("connector_id");
            entity.Property(e => e.ConnectedPortId).HasColumnName("connected_port_id");
            entity.Property(e => e.IMObjID).HasColumnName("im_obj_id");
            entity.Property(e => e.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
            entity.Property(e => e.ComplementaryID).HasColumnName("complementary_id");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<ActivePort> entity);
    }
}


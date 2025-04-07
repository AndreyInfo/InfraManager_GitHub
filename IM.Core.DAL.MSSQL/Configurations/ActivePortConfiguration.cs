using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using IM.Core.DAL.Microsoft.SqlServer;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

public partial class ActivePortConfiguration : ActivePortConfigurationBase
{
    protected override string PrimaryKeyName => "PK_Порт активный";
    protected override string Schema => Options.Scheme;
    protected override string TableName => "Порт активный";
    protected override string IDDefaultValue => "nextval('ActivePort_ID_Seq')";
    protected override string IMObjIDDefaultValue => "newid()";
    protected override string IMObjIDIndexName => "IX_ActivePort_IMObjID";
    protected override string OutletPanelIDIndexName => "IX_ActivePort_OutletPanelID";

    protected override void AdditionalConfig(EntityTypeBuilder<ActivePort> entity)
    {
        entity.Property(e => e.ID).HasColumnName("Идентификатор");
        entity.Property(e => e.PortName).HasColumnName("Name");
        entity.Property(e => e.JackTypeID).HasColumnName("JackTypeID");
        entity.Property(e => e.TechnologyTypeID).HasColumnName("TechnologyID");
        entity.Property(e => e.PortAddress).HasColumnName("MAC-адрес");
        entity.Property(e => e.PortIPX).HasColumnName("IPX сеть");
        entity.Property(e => e.GroupNumber).HasColumnName("Номер группы");
        entity.Property(e => e.PortSpeed).HasColumnName("Скорость");
        entity.Property(e => e.PortVLAN).HasColumnName("Номер VLAN");
        entity.Property(e => e.PortFilter).HasColumnName("Filter");
        entity.Property(e => e.PortState).HasColumnName("Состояние");
        entity.Property(e => e.PortStatus).HasColumnName("PortStatusID");
        entity.Property(e => e.PortModule).HasColumnName("PortModule");
        entity.Property(e => e.SlotNumber).HasColumnName("Номер слота");
        entity.Property(e => e.Description).HasColumnName("Описание");
        entity.Property(e => e.Note).HasColumnName("Примечание");
        entity.Property(e => e.IMObjID).HasColumnName("IMObjID");
        entity.Property(e => e.PortNumber).HasColumnName("Номер порта");
        entity.Property(e => e.ActiveEquipmentID).HasColumnName("ИД активного устройства");
        entity.Property(e => e.Connected).HasColumnName("Connected");
        entity.Property(e => e.Connection1).HasColumnName("Connection1");
        entity.Property(e => e.VisioID).HasColumnName("Visio_ID");
        entity.Property(e => e.ExternalID).HasColumnName("ExternalID");
        entity.Property(e => e.Number).HasColumnName("Номер");
        entity.Property(e => e.ClassID).HasColumnName("ClassID");
        entity.Property(e => e.TelephoneLineTypeID).HasColumnName("TelephoneLineTypeID");
        entity.Property(e => e.TelephoneNumber).HasColumnName("TelephoneNumber");
        entity.Property(e => e.TelephoneCategoryID).HasColumnName("TelephoneCategoryID");
        entity.Property(e => e.VoiceMail).HasColumnName("VoiceMail");
        entity.Property(e => e.RingGroup).HasColumnName("RingGroup");
        entity.Property(e => e.PickUpGroup).HasColumnName("PickUpGroup");
        entity.Property(e => e.HuntingGroup).HasColumnName("HuntingGroup");
        entity.Property(e => e.PermisionGroup).HasColumnName("PermisionGroup");
        entity.Property(e => e.PageGroup).HasColumnName("PageGroup");
        entity.Property(e => e.Connection).HasColumnName("Connection");
        entity.Property(e => e.ConnectorID).HasColumnName("ConnectorID");
        entity.Property(e => e.ConnectedPortId).HasColumnName("ConnectedPortID");
        entity.Property(e => e.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
        entity.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<ActivePort> entity);
}
﻿using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class ActivePortConfigurationBase : IEntityTypeConfiguration<ActivePort>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string Schema { get; }
    protected abstract string TableName { get; }
    protected abstract string IDDefaultValue { get; }
    protected abstract string IMObjIDDefaultValue { get; }
    protected abstract string IMObjIDIndexName { get; }
    protected abstract string OutletPanelIDIndexName { get; }

    protected abstract void AdditionalConfig(EntityTypeBuilder<ActivePort> entity);

    public void Configure(EntityTypeBuilder<ActivePort> entity)
    {
        entity.ToTable(TableName, Schema);

        entity.HasKey(x => x.ID).HasName(PrimaryKeyName);

        entity.Property(e => e.ID).HasDefaultValueSql(IDDefaultValue); ;
        entity.Property(e => e.PortName).IsRequired(false).HasMaxLength(250);
        entity.Property(e => e.JackTypeID).IsRequired(false);
        entity.Property(e => e.TechnologyTypeID).IsRequired(false);
        entity.Property(e => e.PortAddress).IsRequired(false).HasMaxLength(23);
        entity.Property(e => e.PortIPX).IsRequired(false).HasMaxLength(11);
        entity.Property(e => e.GroupNumber).IsRequired(false).HasMaxLength(50);
        entity.Property(e => e.PortSpeed).IsRequired(false).HasMaxLength(50);
        entity.Property(e => e.PortVLAN).IsRequired(false);
        entity.Property(e => e.PortFilter).IsRequired(false).HasMaxLength(255);
        entity.Property(e => e.PortState).IsRequired(false).HasDefaultValueSql("0");
        entity.Property(e => e.PortStatus).IsRequired(true);
        entity.Property(e => e.PortModule).IsRequired(false);
        entity.Property(e => e.SlotNumber).IsRequired(false);
        entity.Property(e => e.Description).IsRequired(false).HasMaxLength(255);
        entity.Property(e => e.Note).IsRequired(false).HasMaxLength(255);
        entity.Property(e => e.IMObjID).IsRequired(false);
        entity.Property(e => e.ActiveEquipmentID).IsRequired(false).HasDefaultValueSql("0");
        entity.Property(e => e.PortNumber).IsRequired(false).HasDefaultValueSql("0");
        entity.Property(e => e.Connected).IsRequired(false).HasDefaultValueSql("0");
        entity.Property(e => e.Connection1).IsRequired(false).HasMaxLength(400);
        entity.Property(e => e.VisioID).IsRequired(false).HasMaxLength(50);
        entity.Property(e => e.ExternalID).IsRequired(true).HasMaxLength(50).HasDefaultValueSql("''");
        entity.Property(e => e.Number).IsRequired(false);
        entity.Property(e => e.ClassID).IsRequired(true);
        entity.Property(e => e.TelephoneLineTypeID).IsRequired(true).HasDefaultValueSql("0"); ;
        entity.Property(e => e.TelephoneNumber).IsRequired(false).HasMaxLength(10);
        entity.Property(e => e.TelephoneCategoryID).IsRequired(true).HasDefaultValueSql("4294ae18-6161-40f8-8f44-b7798d4ee0ef"); ;
        entity.Property(e => e.VoiceMail).IsRequired(false);
        entity.Property(e => e.RingGroup).IsRequired(false);
        entity.Property(e => e.PickUpGroup).IsRequired(false);
        entity.Property(e => e.HuntingGroup).IsRequired(false);
        entity.Property(e => e.PermisionGroup).IsRequired(false);
        entity.Property(e => e.PageGroup).IsRequired(false);
        entity.Property(e => e.Connection).IsRequired(false).HasMaxLength(400);
        entity.Property(e => e. ConnectorID).IsRequired(false);
        entity.Property(e => e.ConnectedPortId).IsRequired(false);
        entity.Property(e => e.PeripheralDatabaseID).IsRequired(false);
        entity.Property(e => e.ComplementaryID).IsRequired(false);

        entity.HasIndex(x => x.IMObjID).HasDatabaseName(IMObjIDIndexName);
        entity.HasIndex(x => x.ActiveEquipmentID).HasDatabaseName(OutletPanelIDIndexName);

        AdditionalConfig(entity);
    }
}
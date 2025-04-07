using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Events;
using InfraManager.DAL.EntityConfigurations;

namespace IM.Core.DAL.PGSQL.Configurations;

public class EventConfiguration : EventConfigurationBase
{
    protected override string ParentEventForeignKeyName => "fk_event_event";

    protected override string SubjectForeignKey => "event_id";

    protected override string SubjectForeignKeyName => "fk_event_subject_event";

    protected override string IXEventOperationID => "ix_event_operation_id";

    protected override string IXEventParentID => "ix_event_parent_id";

    protected override string KeyName => "pk_event";

    protected override void ConfigureDbProvider(EntityTypeBuilder<Event> entity)
    {
        entity.ToTable("event", Options.Scheme);

        entity.Property(e => e.Id).HasColumnName("id");
        entity.Property(e => e.Date).HasColumnName("date").HasColumnType("datetime");
        entity.Property(e => e.InsertOrder).HasColumnName("insert_order");
        entity.Property(e => e.Message).HasColumnName("message");
        entity.Property(e => e.OperationID).HasColumnName("operation_id");
        entity.Property(e => e.ParentID).HasColumnName("parent_id");
        entity.Property(e => e.UserId).HasColumnName("user_id");
    }
}
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Events;
using InfraManager.DAL.EntityConfigurations;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

public class EventConfiguration : EventConfigurationBase
{
    protected override string ParentEventForeignKeyName => "FK_Event_Event";

    protected override string SubjectForeignKey => "EventID";

    protected override string SubjectForeignKeyName => "FK_EventSubject_Event";

    protected override string IXEventOperationID => "IX_Event_OperationID";

    protected override string IXEventParentID => "IX_Event_ParentID";

    protected override string KeyName => "PK_Event";

    protected override void ConfigureDbProvider(EntityTypeBuilder<Event> entity)
    {
        entity.ToTable("Event", "dbo");

        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property(e => e.Date).HasColumnName("Date").HasColumnType("datetime");
        entity.Property(e => e.InsertOrder).HasColumnName("InsertOrder");
        entity.Property(e => e.Message).HasColumnName("Message");
        entity.Property(e => e.OperationID).HasColumnName("OperationID");
        entity.Property(e => e.ParentID).HasColumnName("ParentID");
        entity.Property(e => e.UserId).HasColumnName("UserID");
    }
}

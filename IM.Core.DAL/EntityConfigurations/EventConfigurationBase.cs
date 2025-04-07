using InfraManager.DAL.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class EventConfigurationBase : IEntityTypeConfiguration<Event>
{
    protected abstract string ParentEventForeignKeyName { get; }
    protected abstract string SubjectForeignKey { get; }
    protected abstract string SubjectForeignKeyName { get; }
    protected abstract string IXEventOperationID { get; }
    protected abstract string IXEventParentID { get; }
    protected abstract string KeyName { get; }
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.Id).HasName(KeyName);

        builder.HasIndex(e => e.OperationID, IXEventOperationID);
        builder.HasIndex(e => e.ParentID, IXEventParentID);

        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.InsertOrder).ValueGeneratedOnAdd();
        builder.Property(e => e.Message).IsRequired(false).HasMaxLength(Event.MessageMaxLength);

        ConfigureDbProvider(builder);

        builder.HasOne(d => d.Parent)
            .WithMany()
            .HasForeignKey(d => d.ParentID)
            .HasConstraintName(ParentEventForeignKeyName);

        builder.HasOne(u => u.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .HasPrincipalKey(u => u.IMObjID);

        builder.HasMany(x => x.ChildEvents).WithOne(x => x.Parent).HasForeignKey(x => x.ParentID)
            .HasConstraintName(ParentEventForeignKeyName);
    }

    protected abstract void ConfigureDbProvider(EntityTypeBuilder<Event> builder);

}

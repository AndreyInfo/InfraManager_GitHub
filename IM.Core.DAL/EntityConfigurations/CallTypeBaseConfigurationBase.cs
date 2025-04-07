using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class CallTypeBaseConfigurationBase : IEntityTypeConfiguration<CallType>
{
    protected abstract void ConfigureDatabase(EntityTypeBuilder<CallType> builder);

    protected abstract string PrimaryKeyName { get; }
    protected abstract string UI_Name_ParentCallTypeID { get; }
    
    public void Configure(EntityTypeBuilder<CallType> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.HasIndex(x => new { x.Name, x.ParentCallTypeID }, UI_Name_ParentCallTypeID).IsUnique();
        
        builder.Property(x => x.ID).ValueGeneratedNever();
        builder.Property(x => x.Name).HasMaxLength(250).IsRequired();
        builder.Property(x => x.EventHandlerName).HasMaxLength(250);
        builder.Property(x => x.WorkflowSchemeIdentifier).HasMaxLength(250);
        
        builder.IsMarkableForDelete();

        builder.HasOne(x => x.Parent)
            .WithMany()
            .HasForeignKey(x => x.ParentCallTypeID);
        
        ConfigureDatabase(builder);
    }
}
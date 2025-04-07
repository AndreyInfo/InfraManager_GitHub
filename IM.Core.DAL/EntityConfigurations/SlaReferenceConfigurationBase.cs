using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class SlaReferenceConfigurationBase : IEntityTypeConfiguration<SLAReference>
{
    protected abstract string UISlaSlaReference { get; }
    public void Configure(EntityTypeBuilder<SLAReference> builder)
    {
        builder.HasKey(x => new { x.SLAID, x.ObjectID });
        builder.Property(x => x.TimeZoneID).HasMaxLength(250);

        builder.HasIndex(x => new { x.ObjectID, x.SLAID }, UISlaSlaReference).IsUnique();
        builder.HasOne(x => x.CalendarWorkSchedule).WithMany().HasForeignKey(x => x.CalendarWorkScheduleID).IsRequired(false);
        builder.HasOne(x => x.TimeZone).WithMany().HasForeignKey(x => x.TimeZoneID).IsRequired(false);
        ConfigureDatabase(builder);
    }
    
    
    protected abstract void ConfigureDatabase(EntityTypeBuilder<SLAReference> builder);
}
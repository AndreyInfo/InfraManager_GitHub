using InfraManager.DAL.FormBuilder;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class ServiceLevelAgreementConfigurationBase : IEntityTypeConfiguration<ServiceLevelAgreement>
{

    protected abstract string KeyPK { get; }
    protected abstract string UIName { get; }
    protected abstract string FormForeignKey { get; }
    
    public void Configure(EntityTypeBuilder<ServiceLevelAgreement> builder)
    {
        builder.HasKey(x => x.ID).HasName(KeyPK);
        builder.Property(x => x.Name).HasMaxLength(250).IsRequired(false);
        builder.Property(x => x.Note).IsRequired(false);
        builder.Property(x => x.Number).HasMaxLength(250).IsRequired(false);
        builder.Property(x => x.TimeZoneID).HasMaxLength(250).IsRequired(false);

        builder.HasIndex(x => x.Name, UIName).IsUnique();
        
        builder.HasMany(x => x.References).WithOne().HasForeignKey(x => x.SLAID).OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ServiceReferences).WithOne().HasForeignKey(x => x.SLAID)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Rules).WithOne().HasForeignKey(x => x.ServiceLevelAgreementID).OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.OrganizationItemGroups).WithOne().HasForeignKey(x => x.ID);
        builder.HasOne(x => x.CalendarWorkSchedule).WithMany().HasForeignKey(x => x.CalendarWorkScheduleID);
        builder.HasOne(x => x.TimeZone).WithMany().HasForeignKey(x => x.TimeZoneID);
        builder.HasOne<Form>().WithMany().HasForeignKey(x => x.FormID).HasConstraintName(FormForeignKey);
        
        ConfigureDataProvider(builder);
    }
    
    protected abstract void ConfigureDataProvider(EntityTypeBuilder<ServiceLevelAgreement> builder);
}
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class ServiceAttendanceConfigurationBase : IEntityTypeConfiguration<ServiceAttendance>
{
    protected abstract string KeyName { get; }
    protected abstract string NameUI { get; }
    protected abstract string ServiceIDIX { get; }
    protected abstract string ServiceFK { get; }
    protected abstract string DefaultValueID { get; }
    
    protected abstract string FormForeignKey { get; }

    public void Configure(EntityTypeBuilder<ServiceAttendance> builder)
    {
        builder.HasKey(x => x.ID).HasName(KeyName);

        builder.HasIndex(c => new { c.ServiceID, c.Name }, NameUI).IsUnique();
        builder.HasIndex(c => new { c.ServiceID }, ServiceIDIX);


        builder.Property(x => x.ID).ValueGeneratedOnAdd().HasDefaultValueSql(DefaultValueID);
        builder.Property(x => x.Note).IsRequired(false);
        builder.Property(x => x.Parameter).IsRequired(true);

        builder.Property(x => x.Name)
            .HasMaxLength(250)
            .IsRequired(true);

        builder.Property(x => x.WorkflowSchemeIdentifier)
            .HasMaxLength(250)
            .IsRequired(false);

        builder.Property(x => x.ExternalID)
            .IsRequired(true)
            .HasMaxLength(250);

        builder.Property(x => x.Summary)
            .HasMaxLength(250)
            .IsRequired(true)
            .HasDefaultValueSql("('')");


        builder.HasOne(x => x.Service)
            .WithMany()
            .HasForeignKey(d => d.ServiceID)
            .HasConstraintName(ServiceFK);

        builder.HasOne(x => x.Form)
          .WithMany()
          .HasForeignKey(c => c.FormID)
          .HasConstraintName(FormForeignKey)
          .IsRequired(false);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<ServiceAttendance> builder);
}

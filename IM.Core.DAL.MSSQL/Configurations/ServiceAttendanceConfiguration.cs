using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class ServiceAttendanceConfiguration : ServiceAttendanceConfigurationBase
{
    protected override string KeyName => "PK_ServiceAttendance";

    protected override string NameUI => "UI_Service_Attendance_Name_Into_Service";

    protected override string ServiceFK => "FK_ServiceAttendance_Service";

    protected override string DefaultValueID => "NEWID()";

    protected override string FormForeignKey => "FK_ServiceAttendance_FormID";

    protected override string ServiceIDIX => "IX_ServiceAttendance_ServiceID";

    protected override void ConfigureDataBase(EntityTypeBuilder<ServiceAttendance> builder)
    {
        builder.ToTable("ServiceAttendance", Options.Scheme);


        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.Parameter).HasColumnName("Parameter");
        builder.Property(x => x.Agreement).HasColumnName("Agreement");
        builder.Property(x => x.ServiceID).HasColumnName("ServiceID");
        builder.Property(x => x.WorkflowSchemeIdentifier).HasColumnName("WorkflowSchemeIdentifier");
        builder.Property(x => x.ExternalID).HasColumnName("ExternalID");
        builder.Property(x => x.Summary).HasColumnName("Summary");
        builder.Property(x => x.FormID).HasColumnName("FormID");

        builder.Property(x => x.Note)
            .HasColumnType("ntext")
            .HasColumnName("Note");

        builder.Property(x => x.Type)
            .HasColumnType("tinyint")
            .HasColumnName("Type");

        builder.Property(x => x.State)
            .HasColumnType("tinyint")
            .HasColumnName("State");

        builder.Property(x => x.RowVersion)
            .HasColumnType("timestamp")
            .IsRowVersion()
            .HasColumnName("RowVersion");
    }
}

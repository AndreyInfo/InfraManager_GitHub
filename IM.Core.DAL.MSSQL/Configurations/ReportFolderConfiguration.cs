using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class ReportFolderConfiguration : ReportFolderConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_ReportFolders";

        protected override void ConfigureDatabase(EntityTypeBuilder<ReportFolder> builder)
        {
            builder.ToTable("ReportFolder", "dbo");

            builder.Property(x => x.ID)
                .HasDefaultValueSql("NEWID()")
                .HasColumnName("ID");

            builder.Property(x => x.SecurityLevel)
                .HasColumnName("SecurityLevel")
                .IsRequired();

            builder.Property(x => x.ReportFolderID)
              .HasColumnName("ReportFolderID")
              .IsRequired();

            builder.Property(x => x.RowVersion)
               .IsRowVersion()
               .IsRequired()
               .HasColumnType("timestamp")
               .HasColumnName("RowVersion");
        }
    }
}

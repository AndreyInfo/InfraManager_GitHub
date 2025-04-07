using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class ReportConfiguration : ReportConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_Reports";
        protected override void ConfigureDatabase(EntityTypeBuilder<Reports> builder)
        {
            builder.ToTable("Report", "dbo");

            builder.Property(x => x.ID)
                .HasDefaultValueSql("NEWID()")
                .HasColumnName("ID");

            builder.Property(x => x.SecurityLevel)
                .HasColumnName("SecurityLevel");

            builder.Property(x => x.DateCreated)
              .HasColumnName("DateCreated");

            builder.Property(x => x.DateModified)
              .HasColumnName("DateModified");

            builder.Property(x => x.Data)
              .HasColumnName("Data");

            builder.Property(x => x.ReportFolderID)
              .HasColumnName("ReportFolderID");

            builder.Property(x => x.RowVersion)
               .IsRowVersion()
               .IsRequired()
               .HasColumnType("timestamp")
               .HasColumnName("RowVersion");
        }
    }
}

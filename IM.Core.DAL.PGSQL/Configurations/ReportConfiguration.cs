using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace InfraManager.DAL.Postgres.Configurations
{
    internal class ReportConfiguration : ReportConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_reports";

        protected override void ConfigureDatabase(EntityTypeBuilder<Reports> builder)
        {
            builder.ToTable("report", Options.Scheme);

            builder.Property(x => x.ID)
                .HasColumnName("id");

            builder.Property(x => x.Name)
                .HasColumnName("name");

            builder.Property(x => x.Note)
                .HasColumnName("note");

            builder.Property(x => x.SecurityLevel)
                .HasColumnName("security_level");

            builder.Property(x => x.DateCreated)
                .HasColumnName("date_created");

            builder.Property(x => x.DateModified)
                .HasColumnName("date_modified");

            builder.Property(x => x.Data)
                .HasColumnName("data");

            builder.Property(x => x.ReportFolderID)
                .HasColumnName("report_folder_id");

            builder.Property(x => x.RowVersion)
                .HasColumnName("row_version");

            builder.HasXminRowVersion(e => e.RowVersion);
        }
    }
}
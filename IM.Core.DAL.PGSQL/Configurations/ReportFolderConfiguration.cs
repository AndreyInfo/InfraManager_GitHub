using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Core.DAL.Postgres;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class ReportFolderConfiguration : ReportFolderConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_report_folders";

        protected override void ConfigureDatabase(EntityTypeBuilder<ReportFolder> builder)
        {
            builder.ToTable("report_folder", Options.Scheme);

            builder.Property(x => x.ID)
                .HasDefaultValueSql("NEWID()")
                .HasColumnName("id");

            builder.Property(x => x.SecurityLevel)
                .HasColumnName("security_level")
                .IsRequired();

            builder.Property(x => x.ReportFolderID)
                .HasColumnName("report_folder_id")
                .IsRequired();

            builder.Property(x => x.Note)
                .HasColumnName("note");

            builder.Property(x => x.Name)
                .HasColumnName("name");

            builder.Property(x => x.RowVersion)
                .HasColumnName("row_version");
            builder.HasXminRowVersion(e => e.RowVersion);
        }
    }
}
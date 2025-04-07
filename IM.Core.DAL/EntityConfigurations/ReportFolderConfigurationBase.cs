using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class ReportFolderConfigurationBase : IEntityTypeConfiguration<ReportFolder>
    {
        public void Configure(EntityTypeBuilder<ReportFolder> builder)
        {
            builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

            builder.Property(x => x.Name)
                .HasMaxLength(125)
                .IsRequired();

            builder.Property(x => x.Note)
                .HasMaxLength(500)
                .IsRequired();

            builder.HasMany(x => x.Childs).WithOne()
                .HasForeignKey(x => x.ReportFolderID);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<ReportFolder> builder);

        protected abstract string PrimaryKeyName { get; }
    }
}

using InfraManager.DAL.Software;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class SoftwareModelDependencyConfigurationBase : IEntityTypeConfiguration<SoftwareModelDependency>
    {
        protected abstract string KeyName { get; }
        protected abstract string ForeignKeyChildSoftwareModel { get; }
        protected abstract string ForeignKeyParentSoftwareModel { get; }


        public void Configure(EntityTypeBuilder<SoftwareModelDependency> builder)
        {
            builder.HasKey(e => new { e.ParentSoftwareModelID, e.ChildSoftwareModelID }).HasName(KeyName);

            builder.Property(e => e.ParentSoftwareModelID).IsRequired(true);
            builder.Property(e => e.ChildSoftwareModelID).IsRequired(true);
            builder.Property(e => e.Type).IsRequired(true);

            builder.HasOne(d => d.SoftwareModelParent)
                .WithMany()
                .HasForeignKey(d => d.ParentSoftwareModelID)
                .HasConstraintName(ForeignKeyParentSoftwareModel);

            builder.HasOne(d => d.SoftwareModelChild)
                .WithOne()
                .HasForeignKey<SoftwareModelDependency>(d => d.ChildSoftwareModelID)
                .HasConstraintName(ForeignKeyChildSoftwareModel);

            ConfigureDataBase(builder);

        }
        protected abstract void ConfigureDataBase(EntityTypeBuilder<SoftwareModelDependency> builder);

    }
}

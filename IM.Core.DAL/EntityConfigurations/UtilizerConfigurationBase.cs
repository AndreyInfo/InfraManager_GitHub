using InfraManager.DAL.Asset;
using InfraManager.DAL.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class UtilizerConfigurationBase : IEntityTypeConfiguration<Utilizer>
    {
        public void Configure(EntityTypeBuilder<Utilizer> builder)
        {
            builder.Property(c => c.ID).HasColumnName("id");
            builder.Property(c => c.Name).HasColumnName("name");
            builder.Property(c => c.ClassID).HasColumnName("class_id");

            ConfigureDbProvider(builder);
        }

        protected abstract void ConfigureDbProvider(EntityTypeBuilder<Utilizer> builder);
    }
}

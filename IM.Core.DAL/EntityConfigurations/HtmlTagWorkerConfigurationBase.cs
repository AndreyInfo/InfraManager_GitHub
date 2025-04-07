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
    public abstract class HtmlTagWorkerConfigurationBase : IEntityTypeConfiguration<HtmlTagWorker>
    {
        #region configuration
        public void Configure(EntityTypeBuilder<HtmlTagWorker> builder)
        {
            builder.HasKey(e => e.ID).HasName(PrimaryKeyName);
            builder.Property(x => x.Name).HasMaxLength(250);
            builder.Property(x => x.TagName).HasMaxLength(200);
            builder.Property(x => x.Text).HasMaxLength(200);
            builder.Property(x => x.Class).HasMaxLength(200);
            builder.Property(x => x.Style).HasMaxLength(200);
            builder.Property(x => x.AttrName).HasMaxLength(200);
            builder.Property(x => x.AttrValue).HasMaxLength(200);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<HtmlTagWorker> builder);
        #endregion

        #region Keys
        protected abstract string PrimaryKeyName { get; }
        #endregion
    }
}

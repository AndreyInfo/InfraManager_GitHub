using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Settings;

namespace IM.Core.DAL.PGSQL.Configurations
{
    public class WebUserSplitterSettingsConfiguration : IEntityTypeConfiguration<WebUserSplitterSettings>
    {
        public void Configure(EntityTypeBuilder<WebUserSplitterSettings> entity)
        {
            entity.ToTable("web_splitter_settings", Options.Scheme);

            entity.HasKey(e => new {e.UserID, e.Name}).HasName("pk_web_splitter_settings");

            entity.Property(e => e.UserID)
                .HasColumnName("user_id");

            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");

            entity.Property(e => e.Distance)
                .IsRequired()
                .HasColumnName("distance");
        }
    }
}
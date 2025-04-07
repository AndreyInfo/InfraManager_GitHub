using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Settings;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
  public class WebUserSplitterSettingsConfiguration: IEntityTypeConfiguration<WebUserSplitterSettings>
    {
        public void Configure(EntityTypeBuilder<WebUserSplitterSettings> entity)
        {
            entity.ToTable("WebSplitterSettings","dbo");

            entity.HasKey(e => new { e.UserID, e.Name });

            entity.Property(e => e.UserID)
                .HasColumnName("UserID");

            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("Name");

            entity.Property(e => e.Distance)
                .IsRequired()
                .HasColumnName("Distance");
        }
    }
}

using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    public class EmailQuoteTrimmerConfiguration : EmailQuoteTrimmerConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_EmailQuoteTrimmer";

        protected override void ConfigureDatabase(EntityTypeBuilder<EmailQuoteTrimmer> builder)
        {
            builder.ToTable("EmailQuoteTrimmer", "dbo");
            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.Body).HasColumnName("Body");
            builder.Property(x => x.BodyType).HasColumnName("BodyType");
            builder.Property(x => x.From).HasColumnName("From");
            builder.Property(x => x.FromType).HasColumnName("FromType");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.RowVersion).HasColumnName("RowVersion")
                .HasColumnType("timestamp")
                .IsRowVersion()
                .IsConcurrencyToken();
            builder.Property(x => x.Sequence).HasColumnName("Sequence");
            builder.Property(x => x.Theme).HasColumnName("Theme");
            builder.Property(x => x.ThemeType).HasColumnName("ThemeType");
        }
    }
}

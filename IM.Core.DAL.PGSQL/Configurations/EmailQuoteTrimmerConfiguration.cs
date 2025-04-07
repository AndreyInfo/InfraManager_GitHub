using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Settings;
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
    public class EmailQuoteTrimmerConfiguration : EmailQuoteTrimmerConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_email_quote_trimmer";

        protected override void ConfigureDatabase(EntityTypeBuilder<EmailQuoteTrimmer> builder)
        {
            builder.ToTable("email_quote_trimmer", Options.Scheme);
            builder.HasKey(e => e.ID).HasName(PrimaryKeyName);
            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.Body).HasColumnName("body");
            builder.Property(x => x.BodyType).HasColumnName("body_type");
            builder.Property(x => x.From).HasColumnName("from_");
            builder.Property(x => x.FromType).HasColumnName("from_type");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.HasXminRowVersion(p => p.RowVersion);
            builder.Property(x => x.Sequence).HasColumnName("sequence");
            builder.Property(x => x.Theme).HasColumnName("theme");
            builder.Property(x => x.ThemeType).HasColumnName("theme_type");
        }
    }
}
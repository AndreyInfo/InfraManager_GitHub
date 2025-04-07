using InfraManager.DAL.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class EmailQuoteTrimmerConfigurationBase : IEntityTypeConfiguration<EmailQuoteTrimmer>
    {
        public void Configure(EntityTypeBuilder<EmailQuoteTrimmer> builder)
        {
            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID);
            builder.Property(x => x.Body);
            builder.Property(x => x.BodyType);
            builder.Property(x => x.From);
            builder.Property(x => x.FromType);
            builder.Property(x => x.Name);
            builder.Property(x => x.RowVersion);
            builder.Property(x => x.Sequence);
            builder.Property(x => x.Theme);
            builder.Property(x => x.ThemeType);

            ConfigureDatabase(builder);
        }

        protected abstract string PrimaryKeyName { get; }
        protected abstract void ConfigureDatabase(EntityTypeBuilder<EmailQuoteTrimmer> builder);
    }
}

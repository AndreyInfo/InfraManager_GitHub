using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class ReportConfigurationBase : IEntityTypeConfiguration<Reports>
    {
        public void Configure(EntityTypeBuilder<Reports> builder)
        {
            builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

            builder.Property(x => x.Name)
                .HasMaxLength(125)
                .IsRequired();

            builder.Property(x => x.Note)
                .HasMaxLength(2000)
                .IsRequired();

            builder.HasOne(x => x.Folder)
                .WithMany()
                .HasForeignKey(x => x.ReportFolderID);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<Reports> builder);

        protected abstract string PrimaryKeyName { get; }
    }
}

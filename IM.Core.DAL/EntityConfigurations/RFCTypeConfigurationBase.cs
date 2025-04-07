using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inframanager.DAL.EntityConfigurations
{
    public abstract class RFCTypeConfigurationBase : IEntityTypeConfiguration<ChangeRequestType>
    {
        protected abstract string IndexName { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<ChangeRequestType> entity);

        public void Configure(EntityTypeBuilder<ChangeRequestType> builder)
        {
            builder.HasKey(x => x.ID);
            builder.Property(x => x.ID).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(250);
            builder.IsMarkableForDelete();
            builder.Property(x => x.WorkflowSchemeIdentifier).HasMaxLength(250);
            builder.HasIndex(x => x.Name).IsUnique().HasDatabaseName(IndexName);

            builder.HasOne(x => x.Form)
                .WithMany()
                .HasForeignKey(c => c.FormID)
                .IsRequired(false);

            ConfigureDatabase(builder);
        }
    }
}
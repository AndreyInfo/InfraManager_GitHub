using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{

    public abstract class FormValuesConfiguration : IEntityTypeConfiguration<FormValues>
    {
        protected abstract string PrimaryKeyName { get; }
        protected abstract string FormForeignKeyName { get; }
        protected abstract string FormValuesForeignKeyName { get; }

        public void Configure(EntityTypeBuilder<FormValues> builder)
        {
            builder.Property(x => x.ID).ValueGeneratedOnAdd();
            builder.HasKey(x => x.ID).HasName(PrimaryKeyName);
            builder
               .HasOne(x => x.Form)
               .WithMany()
               .HasForeignKey(x => x.FormBuilderFormID)
               .HasConstraintName(FormForeignKeyName);

            builder
             .HasMany(x => x.Values)
             .WithOne()
             .HasForeignKey(x => x.FormValuesID)
             .HasConstraintName(FormValuesForeignKeyName)
             .OnDelete(DeleteBehavior.Cascade);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<FormValues> builder);

    }
}

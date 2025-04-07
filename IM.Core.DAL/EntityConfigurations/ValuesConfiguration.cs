using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{

    public abstract class ValuesConfiguration : IEntityTypeConfiguration<Values>
    {
        protected abstract string PrimaryKeyName { get; }
        protected abstract string FormFieldForeignKeyName { get; }

        public void Configure(EntityTypeBuilder<Values> builder)
        {
            builder.Property(x => x.ID).ValueGeneratedOnAdd();
            builder.HasKey(x => x.ID).HasName(PrimaryKeyName);
            builder
               .HasOne(x => x.FormField)
               .WithMany()
               .HasForeignKey(x => x.FormFieldID)
               .HasConstraintName(FormFieldForeignKeyName);
            builder.Property(x => x.RowNumber);

          
            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<Values> builder);

    }
}

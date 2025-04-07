using InfraManager.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations
{
    internal class ValuesConfiguration : InfraManager.DAL.EntityConfigurations.ValuesConfiguration
    {
        protected override string PrimaryKeyName => "pk_values";

        protected override string FormFieldForeignKeyName => "fk_values_form_builder_form_tabs_fields";

        protected override void ConfigureDatabase(EntityTypeBuilder<Values> builder)
        {
            builder.ToTable("values", Options.Scheme);            

            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.Order).HasColumnName("order");
            builder.Property(x => x.FormValuesID).HasColumnName("form_values_id");
            builder.Property(x => x.FormFieldID).HasColumnName("form_builder_form_tabs_fields_id");
            builder.Property(x => x.Value).HasColumnName("value");
            builder.HasXminRowVersion(e => e.RowVersion);
            builder.Property(x => x.RowNumber).HasColumnName("row_number");
            builder.Property(x => x.IsReadOnly).HasColumnName("is_read_only");
        }
    }
}

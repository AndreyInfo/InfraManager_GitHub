using InfraManager.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations
{
    internal class FormValuesConfiguration : InfraManager.DAL.EntityConfigurations.FormValuesConfiguration
    {
        protected override string PrimaryKeyName => "pk_form_values";

        protected override string FormForeignKeyName => "fk_form_values_form_builder_form";

        protected override string FormValuesForeignKeyName => "fk_values_form_values";

        protected override void ConfigureDatabase(EntityTypeBuilder<FormValues> builder)
        {
            builder.ToTable("form_values", Options.Scheme);            

            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.FormBuilderFormID).HasColumnName("form_builder_form_id");
            builder.HasXminRowVersion(e => e.RowVersion);
        }
    }
}

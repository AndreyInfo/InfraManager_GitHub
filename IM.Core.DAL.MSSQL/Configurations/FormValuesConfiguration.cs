using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core = InfraManager.DAL.EntityConfigurations;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class FormValuesConfiguration : Core.FormValuesConfiguration
    {
        protected override string PrimaryKeyName => "PK_FormValues";

        protected override string FormForeignKeyName => "FK_FormValues_FormBuilderForm";

        protected override string FormValuesForeignKeyName => "FK_Values_FormValues";

        protected override void ConfigureDatabase(EntityTypeBuilder<FormValues> builder)
        {
            builder.ToTable("FormValues", "dbo");            

            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.FormBuilderFormID).HasColumnName("FormBuilderFormID");
            builder
                .Property(x => x.RowVersion)
                .IsRowVersion()
                .HasColumnType("timestamp")
                .HasColumnName("RowVersion");
        }
    }
}

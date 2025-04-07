using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core = InfraManager.DAL.EntityConfigurations;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class ValuesConfiguration : Core.ValuesConfiguration
    {
        protected override string PrimaryKeyName => "PK_Values";

        protected override string FormFieldForeignKeyName => "FK_Values_WorkflowActivityFormField";

        protected override void ConfigureDatabase(EntityTypeBuilder<Values> builder)
        {
            builder.ToTable("Values", "dbo");            

            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.Order).HasColumnName("Order");
            builder.Property(x => x.FormValuesID).HasColumnName("FormValuesID");
            builder.Property(x => x.FormFieldID).HasColumnName("WorkflowActivityFormFieldID");
            builder.Property(x => x.Value).HasColumnName("Value");
            builder
                .Property(x => x.RowVersion)
                .IsRowVersion()
                .HasColumnType("timestamp")
                .HasColumnName("RowVersion");
            builder.Property(x => x.RowNumber).HasColumnName("RowNumber");
            builder.Property(x => x.IsReadOnly).HasColumnName("IsReadOnly");
        }
    }
}

//using InfraManager.DAL.Message;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class MessageConfiguration : IEntityTypeConfiguration<Message.Message>
    {
        public void Configure(EntityTypeBuilder<Message.Message> builder)
        {
            builder.ToTable("Message", "dbo");
            builder.HasKey(x => x.IMObjID);

            ConfigureColumns(builder);
            ConfigureNavigationProperties(builder);
        }

        private void ConfigureColumns(EntityTypeBuilder<Message.Message> builder)
        {
            builder.Property(x => x.IMObjID).HasColumnName("ID");
            builder.Property(x => x.UtcDateRegistered).HasColumnName("UtcDateRegistered").IsRequired();
            builder.Property(x => x.UtcDateClosed).HasColumnName("UtcDateClosed");
            builder.Ignore(x => x.UtcDateModified);
            builder.Property(x => x.Count).HasColumnName("Count").IsRequired();
            builder.Property(x => x.Type).HasColumnName("Type").IsRequired();
            builder.Property(x => x.SeverityType).HasColumnName("SeverityType").IsRequired();
            builder.Property(x => x.RowVersion).HasColumnName("RowVersion").HasColumnType("timestamp").IsRowVersion();

            builder.Property(x => x.EntityStateID).HasColumnName("EntityStateID").HasMaxLength(50);
            builder.Property(x => x.EntityStateName).HasColumnName("EntityStateName").HasMaxLength(250);
            builder.Property(x => x.WorkflowSchemeID).HasColumnName("WorkflowSchemeID");
            builder.Property(x => x.WorkflowSchemeIdentifier).HasColumnName("WorkflowSchemeIdentifier").HasMaxLength(50);
            builder.Property(x => x.WorkflowSchemeVersion).HasColumnName("WorkflowSchemeVersion").HasMaxLength(50);
            builder.Ignore(x => x.TargetEntityStateID);
        }

        private void ConfigureNavigationProperties(EntityTypeBuilder<Message.Message> builder)
        {
        }
    }
}

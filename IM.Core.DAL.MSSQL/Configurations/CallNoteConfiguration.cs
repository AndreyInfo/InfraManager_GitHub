using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class CallNoteConfiguration : NoteConfigurationBase<Call>
    {
        protected override string PrimaryKeyName => "PK_CallNote";

        protected override void ConfigureDatabase(EntityTypeBuilder<Note<Call>> builder)
        {
            builder.ToTable("CallNote", "dbo");

            builder.Property(x => x.ID).HasColumnName("Id");
            builder.Property(x => x.NoteText).HasColumnName("Note");
            builder.Property(x => x.HTMLNote).HasColumnName("HTMLNote");
            builder.Property(x => x.ParentObjectID).HasColumnName("CallID");
            builder.Property(x => x.Type).HasColumnName("Type");
            builder.Property(x => x.UserID).HasColumnName("UserID");
            builder.Property(x => x.UtcDate).HasColumnName("UtcDate").HasColumnType("datetime");
        }
    }
}

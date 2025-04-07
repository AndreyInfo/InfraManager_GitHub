using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class ProblemNoteConfiguration : NoteConfigurationBase<Problem>
    {
        protected override string PrimaryKeyName => "PK_ProblemNote";

        protected override void ConfigureDatabase(EntityTypeBuilder<Note<Problem>> builder)
        {
            builder.ToTable("ProblemNote", "dbo");

            builder.Property(x => x.ID).HasColumnName("Id");
            builder.Property(x => x.NoteText).HasColumnName("Note");
            builder.Property(x => x.HTMLNote).HasColumnName("HTMLNote");
            builder.Property(x => x.ParentObjectID).HasColumnName("ProblemID");
            builder.Property(x => x.Type).HasColumnName("Type");
            builder.Property(x => x.UserID).HasColumnName("UserID");
            builder.Property(x => x.UtcDate).HasColumnName("UtcDate").HasColumnType("datetime");
        }
    }
}

using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class ObjectNoteInfoConfiguration : ObjectNoteInfoConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_ObjectNoteInfo";

        protected override void ConfigureDatabase(EntityTypeBuilder<ObjectNote> builder)
        {
            builder.ToTable("ObjectNoteInfo", "dbo");

            builder.Property(x => x.ID).HasColumnName("NoteID");
            builder.Property(x => x.ObjectID).HasColumnName("ObjectID");
            builder.Property(x => x.Read).HasColumnName("IsReaded");
            builder.Property(x => x.UserID).HasColumnName("UserID");
        }
    }
}

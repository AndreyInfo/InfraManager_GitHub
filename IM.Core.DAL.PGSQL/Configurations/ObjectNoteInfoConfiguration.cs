using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class ObjectNoteInfoConfiguration : ObjectNoteInfoConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_object_note_info";

        protected override void ConfigureDatabase(EntityTypeBuilder<ObjectNote> builder)
        {
            builder.ToTable("object_note_info", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("note_id");
            builder.Property(x => x.ObjectID).HasColumnName("object_id");
            builder.Property(x => x.Read).HasColumnName("is_readed");
            builder.Property(x => x.UserID).HasColumnName("user_id");
        }
    }
}
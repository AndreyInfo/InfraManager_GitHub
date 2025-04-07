using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class ObjectNoteInfoConfigurationBase : IEntityTypeConfiguration<ObjectNote>
    {
        public void Configure(EntityTypeBuilder<ObjectNote> builder)
        {
            builder.HasKey(x => new { x.ID, x.UserID } ).HasName(PrimaryKeyName);
            builder.Property(x => x.ID).ValueGeneratedNever();
            ConfigureDatabase(builder);
        }

        protected abstract string PrimaryKeyName { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<ObjectNote> builder);
    }
}

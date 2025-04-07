using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class NoteConfigurationBase<TNote> : IEntityTypeConfiguration<Note<TNote>>
    {
        #region configuration

        public void Configure(EntityTypeBuilder<Note<TNote>> builder)
        {
            builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

            builder.Property(x => x.HTMLNote).IsRequired();
            builder.Property(x => x.NoteText).IsRequired();


            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<Note<TNote>> builder);

        #endregion

        #region Keys

        protected abstract string PrimaryKeyName { get; }
        
        #endregion
    }
}

using InfraManager.DAL.Parameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class ParameterEnumConfigurationBase : IEntityTypeConfiguration<ParameterEnum>
    {
        #region configuration
        public void Configure(EntityTypeBuilder<ParameterEnum> builder)
        {
            builder.HasKey(e => e.ID).HasName(PrimaryKeyName);

            builder.Property(x => x.Name).HasMaxLength(250).IsRequired(true);
            builder.Property(x => x.IsTree).IsRequired(true);
            builder.HasIndex(x => x.Name, UI_Name).IsUnique();


            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<ParameterEnum> builder);
        #endregion

        #region Keys
        protected abstract string PrimaryKeyName { get; }
        protected abstract string UI_Name { get; }
        #endregion
    }
}

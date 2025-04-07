using InfraManager.DAL.Parameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class ParameterEnumValueConfigurationBase : IEntityTypeConfiguration<ParameterEnumValue>
    {
        #region configuration
        public void Configure(EntityTypeBuilder<ParameterEnumValue> builder)
        {
            builder.HasKey(e => e.ID).HasName(PrimaryKeyName);

            builder.Property(x => x.Value).HasMaxLength(250).IsRequired();
            builder.Property(x => x.OrderPosition).IsRequired();
            builder.Property(x => x.ParameterEnumID).IsRequired();

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<ParameterEnumValue> builder);
        #endregion

        #region Keys
        protected abstract string PrimaryKeyName { get; }
        protected abstract string ParameterEnumValue_ParameterEnumValue { get; }
        protected abstract string ParameterEnumValue_ParameterEnum { get; }
        #endregion
    }
}

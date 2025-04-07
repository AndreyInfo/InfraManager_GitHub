using InfraManager.DAL.ConfigurationUnits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class NetworkNodeConfigurationBase : IEntityTypeConfiguration<NetworkNode>
    {
        protected abstract string NetworkDeviceFK { get; }
        protected abstract string TerminalDeviceFK { get; }
        protected abstract string DeviceApplicationFK { get; }

        public void Configure(EntityTypeBuilder<NetworkNode> builder)
        {
            builder.Property(x => x.IPAddress).IsRequired(true).HasMaxLength(15);
            builder.Property(x => x.IPMask).IsRequired(true).HasMaxLength(15);

            builder.HasOne(x => x.NetworkDevice)
                .WithMany()
                .HasForeignKey(x => x.NetworkDeviceID)
                .HasPrincipalKey(x => x.ID)
                .IsRequired(false)
                .HasConstraintName(NetworkDeviceFK);

            builder.HasOne(x => x.TermialDevice)
                .WithMany()
                .HasForeignKey(x => x.TerminalDeviceID)
                .HasPrincipalKey(x => x.ID)
                .IsRequired(false)
                .HasConstraintName(TerminalDeviceFK);

            // TODO: После влития КЕ - Приложение
            //builder.HasOne(x => x.DeviceApplication)
            //    .WithMany()
            //    .HasForeignKey(x => x.DeviceApplicationID)
            //    .HasConstraintName(DeviceApplicationFK);

            ConfigureDataBase(builder);
        }
        protected abstract void ConfigureDataBase(EntityTypeBuilder<NetworkNode> builder);
    }
}

using Core = InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using InfraManager.DAL.ServiceDesk.Quality;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class QualityControlConfiguration : Core.QualityControlConfiguration
    {
        protected override string PrimaryKeyName => "PK_QualityContol";
        protected override void ConfigureDatabase(EntityTypeBuilder<QualityControl> builder)
        {
            builder.ToTable("QualityControl", "dbo");

            builder.Property(x => x.TimeSpanInMinutes).HasColumnName("TimeSpanInMinutes");
            builder.Property(x => x.TimeSpanInWorkMinutes).HasColumnName("TimeSpanInWorkMinutes");
            builder.Property(x => x.Type).HasColumnName("Type");
            builder.Property(x => x.CallID).HasColumnName("CallID");
            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.UtcDate).HasColumnName("UtcDate");
        }
    }
}

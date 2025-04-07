using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class ITSystemConfiguration : IEntityTypeConfiguration<ITSystem>
    {

        public void Configure(EntityTypeBuilder<ITSystem> builder)
        {
            builder.ToTable("ITSystem", "dbo");

            builder.HasKey(c => c.ID);

            builder.Property(c => c.Name).HasMaxLength(250);
            builder.Property(x => x.InfrastructureSegmentID);

            builder.Property(c => c.Note).HasMaxLength(500);

            builder.HasOne(x => x.InfrastructureSegment)
                   .WithMany()
                   .HasForeignKey(x => x.InfrastructureSegmentID).HasConstraintName("fk_ITSystem_InfrastructureSegment");
        }
    }
}

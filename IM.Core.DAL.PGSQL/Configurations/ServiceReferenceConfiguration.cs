using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Core.DAL.Postgres;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class ServiceReferenceConfiguration : IEntityTypeConfiguration<ServiceReference>
    {
        public void Configure(EntityTypeBuilder<ServiceReference> builder)
        {
            builder.ToTable("service_reference", Options.Scheme);

            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.ServiceID).HasColumnName("service_id");
            builder.Property(x => x.ClassID).HasColumnName("class_id");
            builder.Property(x => x.ObjectID).HasColumnName("object_id");
        }
    }
}
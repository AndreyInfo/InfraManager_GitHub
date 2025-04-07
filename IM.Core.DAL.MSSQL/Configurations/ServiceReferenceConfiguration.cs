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
    internal class ServiceReferenceConfiguration : IEntityTypeConfiguration<ServiceReference>
    {
        public void Configure(EntityTypeBuilder<ServiceReference> builder)
        {
            builder.ToTable("ServiceReference", "dbo");

            builder.HasKey(x => x.ID);
        }
    }
}

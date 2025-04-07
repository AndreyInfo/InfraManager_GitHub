using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Events;

namespace IM.Core.DAL.PGSQL.Configurations
{
    public partial class EventSubjectParamConfiguration : IEntityTypeConfiguration<EventSubjectParam>
    {
        public void Configure(EntityTypeBuilder<EventSubjectParam> entity)
        {
            entity.ToTable("event_subject_param", Options.Scheme);

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");

            entity.Property(e => e.ParamName)
                .HasColumnName("param_name")
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.ParamNewValue)
                .HasColumnName("param_new_value")
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.ParamOldValue)
                .HasColumnName("param_old_value")
                .HasMaxLength(255);

        }
    }
}

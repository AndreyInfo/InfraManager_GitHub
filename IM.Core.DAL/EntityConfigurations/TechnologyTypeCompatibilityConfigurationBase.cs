using InfraManager.DAL.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class TechnologyTypeCompatibilityConfigurationBase : IEntityTypeConfiguration<TechnologyCompatibilityNode>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string FromForeignKey { get; }
    protected abstract string ToForeignKey { get; }

    public void Configure(EntityTypeBuilder<TechnologyCompatibilityNode> entity)
    {
        entity.HasKey(c => new { c.TechIDFrom, c.TechIDTo }).HasName(PrimaryKeyName);

        entity.HasOne(d => d.TechnologyTypeFrom)
            .WithMany(p => p.TechnologyCompatibilityFrom)
            .HasForeignKey(d => d.TechIDFrom)
            .HasConstraintName(FromForeignKey)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(d => d.TechnologyTypeTo)
            .WithMany(p => p.TechnologyCompatibilityTo)
            .HasForeignKey(d => d.TechIDTo)
            .HasConstraintName(ToForeignKey)
            .OnDelete(DeleteBehavior.Cascade);

        ConfigureDataBase(entity);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<TechnologyCompatibilityNode> entity);
}
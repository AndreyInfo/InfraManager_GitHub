using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class JobTitleConfiguration : JobTitleConfigurationBase
{
    protected override string PrimaryKeyName => "pk_job_title";
    protected override string UniqueNameConstraint => "uk_job_title_name";
    protected override string UniqueIMObjIDConstraint => "uk_job_title_im_obj_id";
    protected override string DefaultValueIMObjID => "gen_random_uuid()";

    protected override void ConfigureDataBase(EntityTypeBuilder<JobTitle> builder)
    {
        builder.ToTable("job_title", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("identificator");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.IMObjID).HasColumnName("im_obj_id");
        builder.Property(x => x.ComplementaryId).HasColumnName("complementary_id");
    }
}
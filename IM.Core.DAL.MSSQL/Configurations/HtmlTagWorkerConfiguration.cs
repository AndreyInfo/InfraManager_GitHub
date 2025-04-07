using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class HtmlTagWorkerConfiguration : HtmlTagWorkerConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_HtmlTagWorker";

        protected override void ConfigureDatabase(EntityTypeBuilder<HtmlTagWorker> builder)
        {
            builder.ToTable("HtmlTagWorker", "dbo");

            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.QuoteTrimmerID).HasColumnName("QuoteTrimmerID");
            builder.Property(x => x.Sequence).HasColumnName("Sequence");
            builder.Property(x => x.TagName).HasColumnName("TagName");
            builder.Property(x => x.Text).HasColumnName("Text");
            builder.Property(x => x.TextType).HasColumnName("TextType");
            builder.Property(x => x.Class).HasColumnName("Class");
            builder.Property(x => x.ClassType).HasColumnName("ClassType");
            builder.Property(x => x.Style).HasColumnName("Style");
            builder.Property(x => x.StyleType).HasColumnName("StyleType");
            builder.Property(x => x.AttrName).HasColumnName("AttrName");
            builder.Property(x => x.AttrValue).HasColumnName("AttrValue");
            builder.Property(x => x.AttrType).HasColumnName("AttrType");
            builder.Property(x => x.LeftID).HasColumnName("LeftID");
            builder.Property(x => x.RightID).HasColumnName("RightID");
            builder.Property(x => x.InnerID).HasColumnName("InnerID");
            builder.Property(x => x.Repeat).HasColumnName("Repeat");
            builder.Property(x => x.Skip).HasColumnName("Skip");
            builder.Property(x => x.InnerNot).HasColumnName("InnerNot");
            builder.Property(x => x.RightNot).HasColumnName("RightNot");
            builder.Property(x => x.LeftNot).HasColumnName("LeftNot");
            builder.Property(x => x.OnlyMe).HasColumnName("OnlyMe");
            builder.Property(x => x.RowVersion).IsRowVersion();
        }
    }
}

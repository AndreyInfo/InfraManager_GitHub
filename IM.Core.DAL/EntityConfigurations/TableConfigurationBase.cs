using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class TableConfigurationBase<Entity> : IEntityTypeConfiguration<Entity> where Entity : class
    {
        protected abstract string TableName { get; }
        protected abstract string TableSchema { get; }
        public void Configure(EntityTypeBuilder<Entity> builder)
        {
            builder.ToTable(TableName, TableSchema);

            ConfigureCommon(builder);
            ConfigureDbProvider(builder);
        }

        /// <summary>
        /// Выполняем общую (не зависимую от БД) настройку
        /// </summary>
        /// <param name="builder"></param>
        protected abstract void ConfigureCommon(EntityTypeBuilder<Entity> builder);

        /// <summary>
        /// Выплняем БД-спцифичную настройку
        /// </summary>
        /// <param name="builder"></param>
        protected abstract void ConfigureDbProvider(EntityTypeBuilder<Entity> builder);

    }
}

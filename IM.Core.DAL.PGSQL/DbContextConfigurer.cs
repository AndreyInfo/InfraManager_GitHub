using InfraManager.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace IM.Core.DAL.PGSQL
{
    /// <summary>
    /// Этот класс конфигурирует соединения с PostgreSQL
    /// </summary>
    public class DbContextConfigurer : IConfigureDbContext
    {
        private readonly string connectionString;

        /// <summary>
        /// Создает экземпляр конфигуратора соединения с PG SQL
        /// </summary>
        /// <param name="connectionString">Строка соединения с БД (Pg Sql)</param>
        public DbContextConfigurer(string connectionString, string schema)
        {
            this.connectionString = connectionString;
            Schema = schema;
        }

        public string Schema { get; }

        public void Configure(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies() // TODO: remove lazy loading proxies since it requires all navigation properties be virtual
                .UseNpgsql(connectionString, serverOptions => serverOptions.CommandTimeout(600))
                .LogTo(Console.WriteLine, global::Microsoft.Extensions.Logging.LogLevel.Error)
                .ConfigureWarnings(x =>
                {
                    x.Ignore(CoreEventId.DetachedLazyLoadingWarning);
                })
                .EnableDetailedErrors();
        }
    }
}

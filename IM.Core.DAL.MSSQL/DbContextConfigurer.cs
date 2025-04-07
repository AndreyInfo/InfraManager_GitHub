using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace InfraManager.DAL.Microsoft.SqlServer
{
    /// <summary>
    /// Этот класс конфигурирует соединения с MS SQL
    /// </summary>
    public class DbContextConfigurer : IConfigureDbContext
    {
        private readonly string connectionString;

        /// <summary>
        /// Создает экземпляр конфигуратора соединения с MS SQL
        /// </summary>
        /// <param name="connectionString">Строка соединения с БД (Ms Sql)</param>
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
                .UseSqlServer(connectionString, serverOptions => serverOptions.CommandTimeout(600))
                .LogTo(Console.WriteLine, LogLevel.Error)
                .ConfigureWarnings(x =>
                {
                    x.Ignore(CoreEventId.DetachedLazyLoadingWarning);
                })
                .EnableDetailedErrors();
        }
    }
}

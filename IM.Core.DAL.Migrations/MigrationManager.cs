using System;

using DbUp;
using DbUp.SqlServer;
using DbUp.Postgresql;
using DbUp.Engine;
using System.ComponentModel.DataAnnotations;
using DbUp.ScriptProviders;

namespace IM.Core.DAL.Migrations
{
	internal enum DB
	{
		MsSql = 1,
		PgSql = 2
    }

	/// <summary>
	/// Класс для управления накатом миграций
	/// </summary>
	internal class MigrationManager
	{
		private const string c_migrationTable = "_ScriptsMigration";
        private readonly int _timeout;
        private readonly string _conString;
        private readonly DB _db;

		private UpgradeEngine? _upgradeEngine;

        public MigrationManager(
			int timeout,
            string conString)
		{
			_timeout = timeout;
			_conString = conString;

			if (conString.Contains("Data Source"))
				_db = DB.MsSql;
			else if (conString.Contains("Server"))
				_db = DB.PgSql;
			else
				throw new Exception("Connection string not found");
        }

		/// <summary>
		/// Создает движок, взависимости от бд
		/// </summary>
		/// <returns></returns>
		public MigrationManager BuildEngine()
		{
            FileSystemScriptOptions scriptOptions = new FileSystemScriptOptions() { IncludeSubDirectories = true };
            var builder = this._db switch
			{
				DB.MsSql => DeployChanges.To
					.SqlDatabase(this._conString)
					.WithTransaction()
					.JournalToSqlTable("dbo", c_migrationTable)
					.WithScriptsFromFileSystem(@$"Scripts/{nameof(DB.MsSql)}", scriptOptions),

				DB.PgSql => DeployChanges.To
					.PostgresqlDatabase(this._conString)
					.WithTransaction()
                    .JournalToPostgresqlTable("im", c_migrationTable)
					.WithScriptsFromFileSystem(@$"Scripts/{nameof(DB.PgSql)}", scriptOptions)

			};

            this._upgradeEngine = builder
					.WithExecutionTimeout(TimeSpan.FromSeconds(this._timeout))
					.LogToConsole()
					.Build();

			return this;
		}

        /// <summary>
        /// После создания движка(ref BuildEngine), накатываем миграции на бд
        /// </summary>
        /// <returns>системный код(-1 ошибка, 0 успех)</returns>
        public int DbUp()
		{
            var resultdb = this._upgradeEngine?.PerformUpgrade();
			var result = resultdb?.Successful ?? false;

            if (!result)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(resultdb?.Error);
                Console.ResetColor();

                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();

            return 0;
        }
    }
}


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.Software.Installation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InfraManager.DAL.Import;

internal class LoadFromDatabase : ILoadFromDatabase, ISelfRegisteredService<ILoadFromDatabase>
{
    private readonly ILogger<LoadFromDatabase> _logger;

    private readonly IContextRequestService _requestService;


    public LoadFromDatabase(ILogger<LoadFromDatabase> logger, IContextRequestService requestService)
    {
        _logger = logger;
        _requestService = requestService;
    }

    private static string GetFields(IEnumerable<string> fieldsData)
    {
        var builder = new StringBuilder();
        foreach (var fieldName in fieldsData)
        {
            builder.Append($"\"{fieldName}\",");
        }

        var commandPart = builder.ToString().TrimEnd(',');
        return commandPart;
    }

    public async IAsyncEnumerable<DBRowData> ImportModelsAsync(IEnumerable<string> fieldNames,
        string tableName,
        string databaseName,
        CancellationToken cancellationToken)
    {
        //подключается к базе данных и загружает данные из таблиц в память в зависимости от настроек
        _logger.LogInformation($"Попытка получения открытого подключения к серверу базы данных");

        var connection = _requestService.GetDbConnection();
        if (connection.State is ConnectionState.Broken or ConnectionState.Closed)
            await connection.OpenAsync(cancellationToken);
        _logger.LogInformation($"Получено");
       
        var database = string.IsNullOrWhiteSpace(databaseName) ? string.Empty : $"\"{databaseName}\"";
        var table = $"\"{tableName}\"";
        
        var isExists = await CheckIfDatabaseExists(connection, database, table, cancellationToken);

        if (!isExists)
        {
            _logger.LogInformation($"Таблица {tableName} в базе {databaseName} не найдена");
            _logger.LogError($"Таблица {tableName} в базе {databaseName} не найдена");
            yield break;
        }

        try
        {
            
            _logger.LogInformation($"Импорт таблицы {tableName}");


            PrintFields(fieldNames);

            _logger.LogInformation("Запуск команды");

            await using var command = connection.CreateCommand();
           
            command.CommandText = $"select * from {database}.{table}";

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            _logger.LogInformation("Получение данных"); 
            var fields = await reader.GetColumnSchemaAsync(cancellationToken);
            // var fieldDictionary = fields.Where(x => fieldNames.Contains(x.ColumnName))
            //     .ToDictionary(x => x.ColumnName, x => x);
            while (await reader.ReadAsync(cancellationToken))
            {
                var row = new DBRowData();
                var count = reader.FieldCount;
                for (int i=0;i< count; i++)
                {
                    var name = fields[i].ColumnName;
                    _logger.LogTrace($"Получение поля {name}");

                    var value = reader.IsDBNull(i)
                                ? null
                                : reader.GetValue(i).ToString();

                    _logger.LogTrace($"Значение поля {value} ordinal {i}");
                   
                    row.Cells[name] = value;
                    _logger.LogTrace($"Значение добавлено в словарь");
                }

                yield return row;
            }
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    private static async Task<bool> CheckIfDatabaseExists(DbConnection connection, string database, string table,
        CancellationToken cancellationToken)
    {
        var checkTableCommandText = $"select 0 from {database}.{table}";
        bool isExists;
        try
        {
            await using var checkCommand = connection.CreateCommand();
            checkCommand.CommandText = checkTableCommandText;
            await using var checkReader = await checkCommand.ExecuteReaderAsync(cancellationToken);
            await checkReader.ReadAsync(cancellationToken);
            isExists = true;
        }
        catch (Exception e)
        {
            isExists = false;
        }

        return isExists;
    }

    private void PrintFields(IEnumerable<string> fieldNames)
    {
        _logger.LogInformation($"Импорт полей:");
        foreach (var field in fieldNames)
        {
            _logger.LogInformation($" {field}");
        }
    }
}
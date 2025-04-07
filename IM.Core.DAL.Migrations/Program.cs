using System.Text.RegularExpressions;
using IM.Core.DAL.Migrations;

if (args is null
    || (args.Length != 2 && args.Length != 3))
    throw new Exception("Set agrs! 1 - connection string, 2 - timeout(sec)");

string connectionString = Regex.Replace(args[0], "^-", "");
int timeout = int.Parse(Regex.Replace(args[1], "^-", ""));

var exitimmediately = true;
if (args.Length == 3)
{
    exitimmediately = bool.Parse(Regex.Replace(args[2], "^-", ""));
}

Console.WriteLine("Start db up");

var dbUpManager = new MigrationManager(timeout, connectionString);

dbUpManager.BuildEngine();

var exitCode = dbUpManager.DbUp();

#if DEBUG
Console.ReadLine();
#endif

if (!exitimmediately)
{
    Console.ReadLine();
}

return exitCode;


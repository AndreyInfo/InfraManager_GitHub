using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.Core
{
    public sealed class MsSqlConnectionParameters
    {
        public string ServerName { get; private set; }
        public string DatabaseName { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }

        public MsSqlConnectionParameters(string serverName, string dataBaseName, string userName, string password)
        {
            ServerName = serverName;
            DatabaseName = dataBaseName;
            UserName = userName;
            Password = password;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    public interface IUserFieldsToDictionaryResolver
    {
        public Dictionary<string, string> Resolve(IUserFieldsModel userFields);
    }
}

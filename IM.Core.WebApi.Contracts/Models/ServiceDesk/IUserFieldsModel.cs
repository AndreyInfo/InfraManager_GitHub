using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk
{
    public class IUserFieldsModel
    {
        public string UserField1 { get; init; }
        public string UserField2 { get; init; }
        public string UserField3 { get; init; }
        public string UserField4 { get; init; }
        public string UserField5 { get; init; }
        public string UserField1Name { get; protected set; }
        public string UserField2Name { get; protected set; }
        public string UserField3Name { get; protected set; }
        public string UserField4Name { get; protected set; }
        public string UserField5Name { get; protected set; }
        public Dictionary<string, string> UserFieldNamesDictionary { get; set; }
    }
}

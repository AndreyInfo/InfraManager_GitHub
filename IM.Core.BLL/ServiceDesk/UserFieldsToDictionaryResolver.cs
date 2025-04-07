using IMSystem;
using InfraManager.BLL.Settings.UserFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    internal class UserFieldsToDictionaryResolver : IUserFieldsToDictionaryResolver
    {
        public Dictionary<string,string> Resolve(IUserFieldsModel userFields)
        {
            var result = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(userFields.UserField1Name) && userFields.UserField1Name != Global.UserField1Name)
                result.Add("UserField1Name", userFields.UserField1Name);
            if (!string.IsNullOrWhiteSpace(userFields.UserField2Name) && userFields.UserField2Name != Global.UserField2Name)
                result.Add("UserField2Name", userFields.UserField2Name);
            if (!string.IsNullOrWhiteSpace(userFields.UserField3Name) && userFields.UserField3Name != Global.UserField3Name)
                result.Add("UserField3Name", userFields.UserField3Name);
            if (!string.IsNullOrWhiteSpace(userFields.UserField4Name) && userFields.UserField4Name != Global.UserField4Name)
                result.Add("UserField4Name", userFields.UserField4Name);
            if (!string.IsNullOrWhiteSpace(userFields.UserField5Name) && userFields.UserField5Name != Global.UserField5Name)
                result.Add("UserField5Name", userFields.UserField5Name);
            return result;
        }
    }
}

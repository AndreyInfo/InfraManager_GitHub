using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Extension
{
    public static class Extensions
    {
        public static string GetFriendlyName(this Enum value)
        {
            var mi = value.GetType()?.GetMember(value.ToString())?.FirstOrDefault();
            if (mi != null)
            {
                var attributes = Attribute.GetCustomAttributes(mi, typeof(InfraManager.BLL.Localization.FriendlyNameAttribute));
                var friendlyAttribut = (InfraManager.BLL.Localization.FriendlyNameAttribute)attributes.FirstOrDefault();
                return friendlyAttribut?.Name;
            }
            return Enum.GetName(value.GetType(), value);
        }
    }
}

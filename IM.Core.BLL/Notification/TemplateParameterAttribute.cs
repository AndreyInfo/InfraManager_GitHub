using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Notification
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class TemplateParameterAttribute : Attribute
    {
        private string _displayName;



        public TemplateParameterAttribute(string displayName)
        {
            _displayName = displayName;
        }



        #region Properties
        public string DisplayName
        {
            get { return _displayName; }
        }
        #endregion
    }
}

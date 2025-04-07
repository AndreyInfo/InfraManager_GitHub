using System;

namespace InfraManager.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = true)]
    public class FriendlyNameAttribute : Attribute
    {
        #region properties
        public virtual string Name { get; protected set; }
        public virtual string CultureName { get; protected set; }
        #endregion


        #region constructors
        public FriendlyNameAttribute(string name)
        {
            this.Name = name;
            this.CultureName = string.Empty;
        }

        public FriendlyNameAttribute(string name, string cultureName)
        {
            this.Name = name;
            this.CultureName = cultureName;
        }
        #endregion
    }
}

using System;

namespace InfraManager.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UserTreeSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? FiltrationObjectID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ObjectClass FiltrationObjectClassID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FiltrationObjectName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public FiltrationTreeTypeEnum FiltrationTreeType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public FiltrationFieldEnum FiltrationField{ get; set; }

        public enum FiltrationFieldEnum
        {
            MOL,
            Owner,
            Utilizer,
        }
        public enum FiltrationTreeTypeEnum
        {
            OrgStructure = 0,
            Location = 1,
            Catalog = 2,
        }
    }
}

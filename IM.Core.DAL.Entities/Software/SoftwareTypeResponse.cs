using System;
using System.Collections.Generic;

namespace InfraManager.DAL.Software
{
    public partial class SoftwareTypeResponse
    {
        /// <summary>
        /// Идентификатор типа ПО
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Ссылка на родительский тип ПО
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// Название типа ПО
        /// </summary>
        public string Name { get; set; }
    }
}
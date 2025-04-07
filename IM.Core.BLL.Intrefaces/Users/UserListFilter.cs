using System;
using System.Collections.Generic;
using InfraManager.BLL.ServiceDesk;

namespace InfraManager.BLL.Users
{    /// <summary>
     /// Модель фильтра списка пользователей
     /// </summary>
    public class UserListFilter : ExecutorListFilter
    {
        /// <summary>
        /// ID подразделения
        /// </summary>
        public Guid? SubdivisionID { get; init; }

        /// <summary>
        /// ID организации
        /// </summary>
        public Guid? OrganizationID { get; init; }
        
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string SearchRequest { get; init; }
        
        /// <summary>
        /// Кол-во записей пропустить
        /// </summary>
        public Guid? ControlsObjectID { get; init; }
        public ObjectClass? ControlsObjectClassID { get; init; }
        public bool ControlsObjectValue { get; init; }
        
        /// <summary>
        /// ID Роли польователя
        /// </summary>
        public Guid? RoleID { get; init; }
        
        /// <summary>
        /// ID Должности польователя Guid
        /// </summary>
        public Guid? PositionIDGuid { get; init; }
        
        /// <summary>
        /// ID Должности польователя int
        /// </summary>
        public int? PositionIDInt { get; init; }
        
        /// <summary>
        /// ID польователя
        /// </summary>
        public Guid? UserID { get; init; }

        /// <summary>
        /// List ID Подразделений
        /// </summary>
        public List<Guid>? SubdivisionIDList { get; init; }
        
        /// <summary>
        /// Вычислять ли подразделения у пользователей в userIDList
        /// </summary>
        public bool GetSubdivisionByUserIDList { get; init; }
        
        /// <summary>
        /// Может ли пользователь быть экспертом БЗ
        /// </summary>
        public bool OnlyKBExpert { get; init; }

        /// <summary>
        /// ID Группы польователя
        /// </summary>
        public Guid? GroupID { get; init; }
    }
}

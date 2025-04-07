using System;

namespace InfraManager.DAL.AccessManagement
{
    /// <summary>
    /// Этот класс представляет сущность Пользователь - Роль
    /// </summary>
    public class UserRole
    {
        protected UserRole()
        {
        }

        /// <summary>
        /// Создает эксземпляр класса UserRole
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="role">Роль</param>
        public UserRole(IUser user, Role role)
        {
            UserID = user.IMObjID;
            RoleID = role.ID;
            Role = role;
        }

        public UserRole(Guid roleID, Guid userID)
        {
            RoleID = roleID;
            UserID = userID;
        }

        /// <summary>
        /// Идентификатор пользователя (IMObjID)
        /// </summary>
        public Guid UserID { get; set; }

        /// <summary>
        /// Идентификатор роли
        /// </summary>
        public Guid RoleID { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public virtual Role Role { get; init; }
    }
}

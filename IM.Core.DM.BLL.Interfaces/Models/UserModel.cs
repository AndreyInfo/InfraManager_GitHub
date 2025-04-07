using System;

namespace IM.Core.DM.BLL.Interfaces.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }

        public string Patronymic { get; set; }

        public bool HasAdminRole { get; set; }

        public string Email { get; set; }

        public string LoginName { get; set; }

        public string Name { get; set; }

        public string PositionName { get; set; }
        
        public byte[] WebPasswordHash { get; set; }

        public bool WebAccessIsGranted { get; set; }

        public string Surname { get; set; }
    }
}

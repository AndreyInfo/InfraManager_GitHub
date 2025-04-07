using System;

namespace InfraManager.BLL.Asset
{
    public class GroupQueueUserDetails
    {
        public int ID { get; init; }
        public Guid UID { get; init; }

        public string Name { get; init; }

        public string Surname { get; init; }

        public string Patronymic { get; init; }

        public string Email { get; init; }

        public string Phone { get; init; }

        public string DepartamentName { get; init; }

        public string RoleName { get; init; }

        public byte[] Photo { get; init; }
        public string FullName { get; set; }
    }
}

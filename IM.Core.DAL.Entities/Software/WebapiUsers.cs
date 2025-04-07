using System;

namespace InfraManager.DAL.Software
{
    public partial class WebApiUsers
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string LastName { get; set; }
        public int? WorkplaceId { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Pager { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Comments { get; set; }
        public string ExternalId { get; set; }
        public int? PositionId { get; set; }
        public Guid? DivisionId { get; set; }
        public bool Removed { get; set; }
        public Guid? ImobjId { get; set; }
    }
}
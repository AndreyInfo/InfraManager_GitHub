using System.Linq.Expressions;
using InfraManager.DAL;

namespace IM.Core.Import.BLL.Interface.Import.Debug
{
    public record UserDebugData 
    {


        #region Identification

        public const int SystemUserId = 0;
        public static readonly Guid SystemUserGlobalIdentifier = Guid.Empty;
        public const int NullUserId = 1;
        public static readonly Guid NullUserGloablIdentifier = new ("00000000-0000-0000-0000-000000000001");

        public int ID { get; set; }
        public Guid IMObjID { get; set; }
        public byte[] RowVersion { get; set; }
        public int? PositionID { get; set; }

        public int? VisioID { get; set; }
        public string ExternalID { get; set; }
        public string SID { get; set; }
        public string Number { get; set; }

        public static int[] SystemUserIds = new[] { SystemUserId, NullUserId };

        public static Expression<Func<User, bool>> ExceptSystemUsers =>
            user => !SystemUserIds.Contains(user.ID);

        #endregion

        #region Details

        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Surname { get; set; }
        public string Initials { get; set; }
        public byte[] Photo { get; set; }
        public string Note { get; set; }
        public string FullName => string.Join(" ", Surname, Name, Patronymic);
        public string Details =>
            string.Join(
                ",",
                (new[]
                {
                            Number,
                            LoginName,
                            Phone,
                            Phone1,
                            Email
                })
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToArray());

        public static string GetFullName(Guid? userId) => throw new NotSupportedException();

        #endregion

        #region Contact Info

        public string Phone { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Phone3 { get; set; }
        public string Phone4 { get; set; }
        public string Fax { get; set; }
        public string Pager { get; set; }
        public string Email { get; set; }

        #endregion

        #region Job Info

        public int? RoomID { get; set; }

        public Guid? SubdivisionID { get; set; }
        
        public int? WorkplaceID { get; set; }

        #endregion

        #region Access Info

        public string LoginName { get; set; }
        public bool Admin { get; set; }
        public bool SupportOperator { get; set; }
        public bool NetworkAdmin { get; set; }
        public bool SupportEngineer { get; set; }
        public bool SupportAdmin { get; set; }
        public bool SDWebAccessIsGranted { get; set; }
        public byte[] SDWebPassword { get; set; }

        #endregion

        #region IMarkableForDelete

        public bool Removed { get; set; }

        public void MarkForDelete()
        {
            Removed = true;
        }

        #endregion

        #region Other

        public Guid? PeripheralDatabaseId { get; set; }
        public int? ComplementaryId { get; set; }
        public Guid? ComplementaryGuidId { get; set; }
        public Guid? CalendarWorkScheduleId { get; set; }
        public bool? IsLockedForOsi { get; set; }
        public Guid? ManagerId { get; set; }
        public string TimeZoneID { get; set; }

        #endregion
    }
}

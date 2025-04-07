using Inframanager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL.OrganizationStructure
{
    [ObjectClassMapping(ObjectClass.Division)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.Subdivision_Add)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.Subdivision_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.Subdivision_View)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
    [OperationIdMapping(ObjectAction.Update, OperationID.Subdivision_Update)]
    public class Subdivision:ILockableForOsi
    {
        private static readonly Guid EmptySubdivisionGlobalIdentifier = new("00000000-0000-0000-0000-000000000001");
        
        public Guid ID { get; set; }
        public Guid OrganizationID { get; set; }
        public string Name { get; set; }
        public Guid? SubdivisionID { get; set; }
        public string Note { get; set; }
        public string ExternalID { get; set; }
        public Guid? PeripheralDatabaseID { get; set; }
        public Guid? ComplementaryID { get; set; }
        public Guid? CalendarWorkScheduleID { get; set; }
        public bool? IsLockedForOsi { get; set; }
        public bool It { get; set; }
        public virtual Organization Organization { get; init; }

        public virtual Subdivision ParentSubdivision { get; set; }
        public virtual ICollection<Subdivision> ChildSubdivisions { get; set; }


        public string FullName => GetFullName();

        private string GetFullName()
        {
            return ParentSubdivision == null
                ? $"{(Organization == null ? "" : $"{Organization.Name} / ")}{Name}"
                : $"{ParentSubdivision.FullName} / {Name}";
        }

        [Obsolete("Use IRepository<User>.Where(u => u.SubdivisionID = subdivisionID) instead")]
        public virtual ICollection<User> Users { get; set; }

        public static string GetFullSubdivisionName(Guid? id) => throw new NotImplementedException();
        public static Expression<Func<Guid?, string>> SubdivisionFullName =>
            id => GetFullSubdivisionName(id);

        public static bool SubdivisionIsSibling(Guid? parentId, Guid? childId) => throw new NotImplementedException();

        /// <summary>
        /// Получить список родительских подразделений вместе с текущим
        /// </summary>
        /// <param name="all"></param>
        /// <returns></returns>
        public IEnumerable<Subdivision> GetBranch(Subdivision[] all)
        {
            var result = new List<Subdivision>();
            FindSubdivisionBranch(all, result, ID);

            return result.AsEnumerable();
        }

        public static Expression<Func<Subdivision, bool>> ExceptEmptySubdivision =>
            subdivision => subdivision.ID != EmptySubdivisionGlobalIdentifier;

        // todo: сама практика не очень хороша, нужно думать, как прикрутить CTE
        public bool IsInItSubdivision(Subdivision[] all)
        {
            var subdivisionBranch = GetBranch(all);
            return subdivisionBranch.Any(a => a.It);
        }

        private static void FindSubdivisionBranch(Subdivision[] all, List<Subdivision> result, Guid fromId)
        {
            var subdivision = all.SingleOrDefault(x => x.ID == fromId);

            if (subdivision == null) // Родительское подразделение удалено, а дочернее - нет (ошибка, но тут падать мы не должны)
            {
                return;
            }

            result.Add(subdivision);

            if (subdivision.SubdivisionID.HasValue
                    && result.All(x => x.ID != subdivision.SubdivisionID.Value)) // Защита от зацикливания в справочнике подразделений
            {
                FindSubdivisionBranch(all, result, subdivision.SubdivisionID.Value); // Идем к корневому подразделению
            }
        }
    }
}
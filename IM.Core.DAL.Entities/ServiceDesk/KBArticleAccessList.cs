using System;
using System.Linq.Expressions;

namespace InfraManager.DAL.ServiceDesk
{
    public class KBArticleAccessList
    {
        public Guid KbArticleID { get; init; }

        public Guid ObjectID { get; init; }

        public ObjectClass ObjectClass { get; init; }

        public string ObjectName { get; set; }

        public bool WithSub { get; set; }

        public static Expression<Func<KBArticleAccessList, bool>> IsAccessedByClass(ObjectClass classID, Guid objectID) =>
            (access) => access.ObjectClass == classID && access.ObjectID == objectID;
    }
}

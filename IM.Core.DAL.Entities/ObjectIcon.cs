using System;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL
{
    public class ObjectIcon
    {
        protected ObjectIcon()
        {
        }

        private ObjectIcon(InframanagerObject objectID)
        {
            ObjectID = objectID.Id;
            ObjectClassID = objectID.ClassId;
        }

        public ObjectIcon(InframanagerObject objectID, string name)
            : this(objectID)
        {
            Name = name;
        }

        public ObjectIcon(InframanagerObject objectID, byte[] content)
            : this(objectID)
        {
            Content = content;
        }

        public int ID { get; }
        public Guid ObjectID { get; }
        public ObjectClass ObjectClassID { get; }
        public string Name { get; set; }
        public byte[] Content { get; set; } // TODO: Add content type jpeg / gif / png etc.
        public bool NoIcon => string.IsNullOrWhiteSpace(Name) && (Content == null || !Content.Any());
        public static Expression<Func<ObjectIcon, bool>> WhereObjectEquals(Guid objectID, ObjectClass objectClassID) =>
            x => x.ObjectID == objectID && x.ObjectClassID == objectClassID;
        public static Expression<Func<ObjectIcon, bool>> WhereObjectEquals(InframanagerObject objectID) =>
            WhereObjectEquals(objectID.Id, objectID.ClassId);
        public static Expression<Func<ObjectIcon, bool>> WhereObjectEquals<T>(T entity) where T : IGloballyIdentifiedEntity =>
            WhereObjectEquals(entity.IMObjID, typeof(T).GetObjectClassOrRaiseError());
    }
}

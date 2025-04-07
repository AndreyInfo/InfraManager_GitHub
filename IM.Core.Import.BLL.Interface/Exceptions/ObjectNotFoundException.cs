using InfraManager;

namespace IM.Core.Import.BLL.Interface.Exceptions
{
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException(string message) : base(message)
        {
        }
    }

    public class ObjectNotFoundException<TKey> : ObjectNotFoundException
    {
        public ObjectNotFoundException(TKey objectId, ObjectClass objectClass)
            : base($"Requested object (class = {objectClass}, id = {objectId}) was either deleted or not added.")
        {
            ObjectId = objectId;
            Class = objectClass;
        }

        public ObjectNotFoundException(TKey objectId, string objectDescription)
            : base($"Requested {objectDescription} (id = {objectId}) was either deleted or not added.")
        {
            ObjectId = objectId;
            Class = ObjectClass.Unknown;
            ObjectDescription = objectDescription;
        }

        public TKey ObjectId { get; }
        public ObjectClass Class { get; }
        public string ObjectDescription { get; }
    }
}

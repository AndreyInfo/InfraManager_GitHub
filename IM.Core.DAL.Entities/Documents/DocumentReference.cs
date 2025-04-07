using System;

namespace InfraManager.DAL.Documents
{
    public class DocumentReference
    {
        protected DocumentReference()
        {
        }

        public DocumentReference(Guid documentID, Guid objectID, ObjectClass objectClassID)
        {
            DocumentID = documentID;
            ObjectID = objectID;
            ObjectClassID = objectClassID;
        }

        public Guid DocumentID { get; }
        public Guid ObjectID { get; }
        public ObjectClass? ObjectClassID { get; } //todo: сначала надо заполнить ClassID в БД для всех файлов добавленных ранее
        public virtual Document Document { get; }
    }
}

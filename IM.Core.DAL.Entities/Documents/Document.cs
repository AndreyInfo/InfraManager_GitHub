using Inframanager;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL.Documents
{
    [ObjectClassMapping(ObjectClass.Document)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.None)]
    [OperationIdMapping(ObjectAction.Update, OperationID.Document_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.Document_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
    public class Document
    {
        public Document(string name,
            string extension,
            DocumentState documentState,
            Guid authorID,
            Guid? ownerID = null,
            string location = "",
            Guid? documentTypeID = null,
            string note = "")
        {
            ID = Guid.NewGuid();
            Name = name;
            Extension = extension;
            Location = location;
            DateModified = DateTime.UtcNow;
            DocumentTypeID = documentTypeID;
            Note = note;
            DocumentState = documentState;
            AuthorID = authorID;
            OwnerID = ownerID;
        }


        public Guid ID { get; init; }

        public int InternalID { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public byte[] Data { get; set; }

        public string Location { get; set; }

        public DateTime DateModified { get; set; }

        public DateTime DateCreated { get; } = DateTime.UtcNow;

        public Guid? DocumentTypeID { get; set; }

        public string Note { get; set; }

        public DocumentState DocumentState { get; set; }

        public Guid AuthorID { get; init; }

        public Guid? OwnerID { get; set; }
        public string EncodedName { get; set; }
    }
}

using System;

namespace InfraManager.Core.Exceptions
{
    [Serializable]
    public sealed class FieldConcurrencyException : ObjectConcurrencyException
    {
        #region constructors
        public FieldConcurrencyException(string fieldName)
            : base(fieldName)
        {
            this.FieldName = fieldName;
        }
        public FieldConcurrencyException(string fieldName, object currentObjectValue)
            : this(fieldName)
        {
            this.CurrentObjectValue = currentObjectValue;
        }
        public FieldConcurrencyException(string fieldName, Guid currentObjectID, int currentObjectClassID, string currentObjectFullName)
            : this(fieldName)
        {
            this.CurrentObjectID = currentObjectID;
            this.CurrentObjectClassID = currentObjectClassID;
            this.CurrentObjectValue = currentObjectFullName;
        }
        #endregion

        #region properties
        public String FieldName { get; set; }

        public Guid? CurrentObjectID { get; set; }
        public int? CurrentObjectClassID { get; set; }
        public object CurrentObjectValue { get; set; }
        #endregion
    }
}

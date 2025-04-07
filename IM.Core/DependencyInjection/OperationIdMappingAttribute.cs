using Inframanager;
using System;

namespace InfraManager
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class OperationIdMappingAttribute : Attribute
    {
        public OperationIdMappingAttribute(ObjectAction action, OperationID operation)
        {
            Action = action;
            Operation = operation;
        }

        public ObjectAction Action { get; }
        public OperationID Operation { get; }
    }
}

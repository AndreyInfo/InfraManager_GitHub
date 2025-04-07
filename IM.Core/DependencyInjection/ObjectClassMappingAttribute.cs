using System;

namespace InfraManager
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ObjectClassMappingAttribute : Attribute
    {
        public ObjectClassMappingAttribute(ObjectClass objectClass)
        {
            ObjectClass = objectClass;
        }

        public ObjectClass ObjectClass { get; }
    }
}

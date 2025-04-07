using System;

namespace InfraManager.UI.Web.ResourceMapping
{
    public static class WebApiResourceExtensions
    {
        public static bool TryGetObjectClass(this WebApiResource resourceID, out ObjectClass classID)
        {
            var resourceHasMatchingClass = Enum.IsDefined(typeof(ObjectClass), (int)resourceID);
            classID = resourceHasMatchingClass
                ? (ObjectClass)Enum.ToObject(typeof(ObjectClass), (int)resourceID) 
                : default(ObjectClass);

            return resourceHasMatchingClass;
        }
    }
}

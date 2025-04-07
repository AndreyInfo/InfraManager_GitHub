using System;

namespace InfraManager.DAL.Location
{
    public abstract class LocationObject : Catalog<int>, ILocationObject
    {
        public Guid IMObjID { get; init; }

        public abstract string GetFullPath();
    }
}

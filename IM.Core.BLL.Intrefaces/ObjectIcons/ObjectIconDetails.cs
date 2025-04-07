using System;

namespace InfraManager.BLL.ObjectIcons
{
    public class ObjectIconDetails : ObjectIconData
    {
        public Guid ObjectID { get; init; }
        public ObjectClass ObjectClassID { get; init; }
    }
}

using System;

namespace Inframanager
{
    public class GenericDependency
    {
        public Type[] TypeParameters { get; init; }
        public Type Service { get; init; }
        public Type Implementation { get; init; }
    }
}

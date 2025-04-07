using System;

namespace InfraManager.ComponentModel
{
    public interface IObject
    {
        int ClassID { get; }
        Guid ID { get; }
        string FullName { get; }

        string ToString();
    }
}

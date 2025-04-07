using System;
using System.Collections.Generic;
using System.Text;

namespace InfraManager
{
    public interface IRole
    {
        Guid ID { get; }
        string Name { get; }
    }
}

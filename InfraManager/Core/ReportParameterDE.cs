using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.Core
{
    public sealed class ReportParameterDE
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Type Type { get; private set; }
        public object Value { get; set; }

        public ReportParameterDE(string name, string description, Type type, object value)
        {
            Name = name;
            Description = description;
            Type = type;
            Value = value;
        }

        public ReportParameterDE(string name, string description, Type type)
        {
            Name = name;
            Description = description;
            Type = type;
        }
    }
}

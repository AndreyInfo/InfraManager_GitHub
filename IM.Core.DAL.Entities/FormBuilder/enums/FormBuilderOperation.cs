using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.FormBuilder.Enums
{
    public enum FormBuilderOperation : int
    {
        Equals = 0,
        NotEquals = 1,
        Filled = 2,
        NotFilled = 3
    }
}

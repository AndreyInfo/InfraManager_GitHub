using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.FormBuilder.Enums
{
    public enum FormBuilderAction : int
    {
        MakeVisible = 0,
        MakeHide = 1,
        AllowedChanges = 2,
        NotAllowedChanges = 3,
        MakeRequired = 4,
        MakeNotRequired = 5
    }
}

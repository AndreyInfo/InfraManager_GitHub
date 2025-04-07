using InfraManager.BLL.Localization;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk
{
    internal class TaskCategoryLookupQuery : CategoryLookupQueryBase
    {
        public TaskCategoryLookupQuery(ILocalizeEnum<Issues> issuesLocalizer) 
            : base(issuesLocalizer, Enum.GetValues<Issues>().Except(new[] { Issues.ChangeRequest }))
        {
        }
    }
}

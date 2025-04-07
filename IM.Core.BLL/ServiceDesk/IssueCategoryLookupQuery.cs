using InfraManager.BLL.Localization;
using InfraManager.DAL.ServiceDesk;
using System;

namespace InfraManager.BLL.ServiceDesk
{
    internal class IssueCategoryLookupQuery : CategoryLookupQueryBase
    {
        public IssueCategoryLookupQuery(ILocalizeEnum<Issues> issuesLocalizer) 
            : base(issuesLocalizer, Enum.GetValues<Issues>())
        {
        }
    }
}

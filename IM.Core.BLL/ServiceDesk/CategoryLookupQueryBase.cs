using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Localization;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk
{
    internal abstract class CategoryLookupQueryBase : ILookupQuery
    {
        private readonly ILocalizeEnum<Issues> _issuesLocalizer;
        private readonly IEnumerable<Issues> _issues;

        protected CategoryLookupQueryBase(ILocalizeEnum<Issues> issuesLocalizer, IEnumerable<Issues> issues)
        {
           _issuesLocalizer = issuesLocalizer;
           _issues = issues;
        }    

        public Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                _issues
                    .Select(
                        issue => new ValueData 
                        { 
                            ID = ((int)issue).ToString(),
                            Info = _issuesLocalizer.Localize(issue)
                        })
                    .ToArray());
        }
    }
}


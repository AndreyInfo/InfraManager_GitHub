using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.Search
{
    public class SolutionByNameCriteria
    {
        public string Text { get; set; }
    }

    public interface ISolutionSearchQuery
    {
        IQueryable<ObjectSearchResult> Query(SolutionByNameCriteria criteria);
    }
}

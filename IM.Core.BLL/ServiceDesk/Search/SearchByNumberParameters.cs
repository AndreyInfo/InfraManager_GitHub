using System.Collections.Generic;

namespace InfraManager.BLL.ServiceDesk.Search
{
    public class SearchByNumberParameters : ServiceDeskSearchParameters
    {
        public SearchByNumberParameters(int number,
            bool shouldSearchFinished,
            IReadOnlyList<ObjectClass> classes) : base(classes)
        {
            Number = number;
            ShouldSearchFinished = shouldSearchFinished;
        }

        public int Number { get; }
        public bool ShouldSearchFinished { get; }
    }
}
using System.Collections.Generic;

namespace InfraManager.DAL.ServiceDesk.Manhours
{
    public interface IHaveManhours
    { 
        int ManhoursInMinutes { get; }
        int ManhoursNormInMinutes { get; }
        IEnumerable<ManhoursWork> Manhours { get; }

        void OnManhoursWorkAdded();

        void IncrementTotalManhours(int value);
        void DecrementTotalManhours(int value);
    }
}
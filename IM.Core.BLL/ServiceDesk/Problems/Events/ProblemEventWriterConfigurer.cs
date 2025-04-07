using Inframanager.BLL.Events;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Problems.Events
{
    internal class ProblemEventWriterConfigurer : IConfigureEventWriter<Problem, Problem>
    {
        public void Configure(IEventWriter<Problem, Problem> writer)
        {
        }
    }
}

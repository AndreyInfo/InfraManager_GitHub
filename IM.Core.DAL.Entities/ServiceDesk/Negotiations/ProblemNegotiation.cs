using System;

namespace InfraManager.DAL.ServiceDesk.Negotiations
{
    public class ProblemNegotiation : Negotiation
    {
        protected ProblemNegotiation()
        {
        }

        internal ProblemNegotiation(Guid problemId) : base(problemId, ObjectClass.Problem)
        {
        }
    }
}

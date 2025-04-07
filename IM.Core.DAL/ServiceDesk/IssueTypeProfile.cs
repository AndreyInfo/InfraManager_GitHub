using AutoMapper;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.DAL.ServiceDesk
{
    internal class IssueTypeProfile : Profile
    {
        public IssueTypeProfile()
        {
            CreateMap<CallType, IssueType>()
                .WithCast(issueType => issueType.Name, callType => CallType.GetFullCallTypeName(callType.ID));
            CreateMap<MassIncidentType, IssueType>()
                .With(issueType => issueType.ID, miType => miType.IMObjID)
                .WithCast(issueType => issueType.Name, miType => miType.Name);
            CreateMap<ProblemType, IssueType>()
                .WithCast(issueType => issueType.Name, problemType => ProblemType.GetFullProblemTypeName(problemType.ID));
            CreateMap<WorkOrderType, IssueType>()
                .WithCast(issueType => issueType.Name, woType => woType.Name);

            CreateMap<Call, IssueTypeReference>().With(x => x.TypeID, call => call.CallTypeID);
            CreateMap<MassIncident, IssueTypeReference>().With(x => x.TypeID, massIncident => massIncident.Type.IMObjID);
            CreateMap<Problem, IssueTypeReference>();
            CreateMap<WorkOrder, IssueTypeReference>();
        }
    }
}

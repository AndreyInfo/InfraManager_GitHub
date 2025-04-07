using InfraManager.BLL.Settings;
using InfraManager.BLL.Workflow;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    public class ProblemWorkflowSchemeIdentifierProvider: 
        ISelectWorkflowScheme<Problem>,
        ISelfRegisteredService<ISelectWorkflowScheme<Problem>>
    {
        private readonly IReadonlyRepository<ProblemType> _problemTypesRepository;
        private readonly ISettingsBLL _settings;
        private readonly IConvertSettingValue<string> _converter;

        public ProblemWorkflowSchemeIdentifierProvider(
            IReadonlyRepository<ProblemType> problemTypesRepository, 
            ISettingsBLL settings, 
            IConvertSettingValue<string> converter)
        {
            _problemTypesRepository = problemTypesRepository;
            _settings = settings;
            _converter = converter;
        }

        public async Task<string> SelectIdentifierAsync(Problem data, CancellationToken cancellationToken = default)
        {
            var allProblemTypes = await _problemTypesRepository
                .With(x => x.Parent)
                .ToArrayAsync(cancellationToken);
            var problemType = allProblemTypes.SingleOrDefault(x => x.ID == data.TypeID);
            var defaultSchemeIdentifier = await _settings.GetValueAsync(SystemSettings.DefaultProblemWorkflowSchemeIdentifier, cancellationToken);
            var workflowSchemeIdentifier = problemType.GetWorkflowSchemeIdentifier();
            
            return string.IsNullOrWhiteSpace(workflowSchemeIdentifier)
                ? _converter.Convert(defaultSchemeIdentifier)
                : workflowSchemeIdentifier;
        }
    }
}

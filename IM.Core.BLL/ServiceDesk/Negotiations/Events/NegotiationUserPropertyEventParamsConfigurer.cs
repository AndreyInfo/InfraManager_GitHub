using Inframanager.BLL.Events;
using InfraManager.BLL.Localization;
using InfraManager.DAL.ServiceDesk.Negotiations;

namespace InfraManager.BLL.ServiceDesk.Negotiations.Events
{
    internal class NegotiationUserPropertyEventParamsConfigurer : IConfigureDefaultEventParamsBuilderCollection<NegotiationUser>
    {
        private readonly ILocalizeEnum<VotingType> _votingTypeLocalizer;

        public NegotiationUserPropertyEventParamsConfigurer(ILocalizeEnum<VotingType> votingTypeLocalizer)
        {
            _votingTypeLocalizer = votingTypeLocalizer;
        }

        public void Configure(IDefaultEventParamsBuildersCollection<NegotiationUser> collection)
        {
            collection
                .HasProperty(x => x.VotingType)
                .HasName("Проголосовал")
                .HasConverter(x => _votingTypeLocalizer.Localize(x));
            collection.HasProperty(x => x.Message).HasName("Комментарий");
        }
    }
}

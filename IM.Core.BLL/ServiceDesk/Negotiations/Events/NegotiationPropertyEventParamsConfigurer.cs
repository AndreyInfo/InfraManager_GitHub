using Inframanager.BLL.Events;
using InfraManager.BLL.Localization;
using InfraManager.DAL.ServiceDesk.Negotiations;

namespace InfraManager.BLL.ServiceDesk.Negotiations.Events
{
    internal class NegotiationPropertyEventParamsConfigurer<TNegotiation> : IConfigureDefaultEventParamsBuilderCollection<TNegotiation>
        where TNegotiation : Negotiation
    {
        private readonly ILocalizeEnum<NegotiationMode> _modeLocalizer;
        private readonly ILocalizeEnum<NegotiationStatus> _statusLocalizer;

        public NegotiationPropertyEventParamsConfigurer(
            ILocalizeEnum<NegotiationMode> modeLocalizer, 
            ILocalizeEnum<NegotiationStatus> statusLocalizer)
        {
            _modeLocalizer = modeLocalizer;
            _statusLocalizer = statusLocalizer;
        }

        public void Configure(IDefaultEventParamsBuildersCollection<TNegotiation> collection)
        {
            collection.HasProperty(x => x.Name).HasName("Название");
            collection
                .HasProperty(x => x.Mode)
                .HasName("Режим")
                .HasConverter(mode => _modeLocalizer.Localize(mode));
            collection
                .HasProperty(x => x.Status)
                .HasName("Статус")
                .HasConverter(status => _statusLocalizer.Localize(status));
            collection.HasProperty(x => x.UtcDateVoteStart).HasName("Начало голосования");
            collection.HasProperty(x => x.UtcDateVoteEnd).HasName("Завершение голосования");
        }
    }
}

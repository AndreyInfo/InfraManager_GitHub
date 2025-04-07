using Inframanager.BLL.Events;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ELP
{
    internal class ELPSettingEventConfigurer : IConfigureEventWriter<ElpSetting, ElpSetting>
    {
        public void Configure(IEventWriter<ElpSetting, ElpSetting> writer)
        {
        }
    }
}
using System;
using Inframanager.BLL.Events;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ELP
{
    internal class ELPSettingSubjectBuilder: EventSubjectBuilderBase<ElpSetting, ElpSetting>
    {
        public ELPSettingSubjectBuilder() : base("Связь между инсталляциями и лицензиями")
        {
        }

        protected override Guid GetID(ElpSetting subject)
        {
            return subject.Id;
        }

        protected override string GetSubjectValue(ElpSetting subject)
        {
            return subject.Name;
        }
    }
}

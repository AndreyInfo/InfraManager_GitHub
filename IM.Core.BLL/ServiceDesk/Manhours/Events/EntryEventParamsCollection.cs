using Inframanager.BLL.Events;
using InfraManager.BLL.Localization;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.Events;
using InfraManager.DAL.ServiceDesk.Manhours;
using InfraManager.ResourcesArea;
using System;
using System.Collections;
using System.Collections.Generic;

namespace InfraManager.BLL.ServiceDesk.Manhours.Events
{
    internal class EntryEventParamsCollection : 
        IEventParameterBuildersCollection<ManhoursEntry, ManhoursWork>,
        IBuildEventSubjectParameter<ManhoursEntry>
    {
        #region .ctor

        private readonly string _paramName;
        private readonly ILocalizeText _localizer;

        public EntryEventParamsCollection(string paramName, ILocalizeText localizer)
        {
            _localizer = localizer;
            _paramName = paramName;
        }

        #endregion

        #region IEventParameterBuildersCollection<ManhoursEntry, ManhoursWork>

        private IEnumerable<IBuildEventSubjectParameter<ManhoursEntry>> EnumerateParamBuilders()
        {
            yield return this;
        }

        public IEnumerator<IBuildEventSubjectParameter<ManhoursEntry>> GetEnumerator()
        {
            return EnumerateParamBuilders().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IBuildEventSubjectParameter<ManhoursEntry>

        private string GetValue(IEntityState state)
        {
            var date = state[nameof(ManhoursEntry.UtcDate)];
            var value = state[nameof(ManhoursEntry.Value)];

            if (date == null || value == null)
            {
                return null;
            }

            return $"{_localizer.ManhoursToString(Global.RU, (int)value)} от {((DateTime)date).ToShortDateString()}";
        }

        public EventSubjectParam Build(EventBuilderContext<ManhoursEntry> context)
        {
            return new EventSubjectParam(_paramName, GetValue(context.OriginalState), GetValue(context.CurrentState));
        }

        #endregion
    }
}

using System;

namespace InfraManager.DAL.Events
{
    public class EventSubjectParam
    {
        public static int MaxLength = 255;
        protected EventSubjectParam()
        {
        }

        public EventSubjectParam(string name, string oldValue, string newValue)
        {
            Id = Guid.NewGuid();
            ParamName = name;
            ParamOldValue = oldValue.Slice(MaxLength);
            ParamNewValue = newValue.Slice(MaxLength);
        }

        public Guid Id { get; init; }
        public string ParamName { get; init; }
        public string ParamOldValue { get; init; }
        public string ParamNewValue { get; init; }
    }
}
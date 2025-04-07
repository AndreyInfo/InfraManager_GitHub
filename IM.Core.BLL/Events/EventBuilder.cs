using InfraManager.BLL.FieldEdit;
using InfraManager.CrossPlatform.WebApi.Contracts.Auth;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Attributes;
using InfraManager.DAL.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Events
{
    internal class EventBuilder : IEventBuilder, ISelfRegisteredService<IEventBuilder>
    {
        enum OperationType
        {
            Add,
            Edit,
            Delete,
        }

        private readonly IFieldManager _fieldManager;
        private readonly IUserContextProvider _userContextProvider;
        public EventBuilder(IFieldManager fieldManager, IUserContextProvider userContextProvider)
        {
            _fieldManager = fieldManager ?? throw new ArgumentNullException(nameof(fieldManager));
            _userContextProvider = userContextProvider ?? throw new ArgumentNullException(nameof(userContextProvider));
        }
        public async Task<BaseResult<Event, EventFaults>> CreateEvent(object oldValue, object newValue, Func<FieldCompareAttribute, string, Func<object, Task<string>>> formatter = null)
        {
            if (newValue == null)
                return new BaseResult<Event, EventFaults>(null, EventFaults.ObjectIsNull);
            var newObjType = newValue.GetType();
            var oldObjType = oldValue?.GetType();
            if (newObjType != (oldObjType ?? newObjType))
                return new BaseResult<Event, EventFaults>(null, EventFaults.ObjectTypeDiffers);

            EntityCompareAttribute compareAttribute = _fieldManager.GetEntityAttribute(newObjType);
            if (compareAttribute == null)
                return new BaseResult<Event, EventFaults>(null, EventFaults.ComparableAttrubutesNotSet);

            var comaprePropertyAttributes = _fieldManager.GetFieldAttributes(newObjType);
            if (!comaprePropertyAttributes.Any())
                return new BaseResult<Event, EventFaults>(null, EventFaults.ComparableAttrubutesNotSet);

            var objID = GetObjectID(newValue, compareAttribute);
            if (objID == null)
                return new BaseResult<Event, EventFaults>(null, EventFaults.ObjectIsNull);
            var paramList = new List<EventSubjectParam>();

            OperationType operationType = oldValue == null ? OperationType.Add : OperationType.Edit;

            if(oldValue != null && comaprePropertyAttributes.Any(x => x.Key.Equals(string.IsNullOrEmpty(compareAttribute.IsDeletedFieldName) ? "IsDeleted" : compareAttribute.IsDeletedFieldName, StringComparison.InvariantCultureIgnoreCase)))
            {
                var delField = comaprePropertyAttributes.First(x => x.Key.Equals(string.IsNullOrEmpty(compareAttribute.IsDeletedFieldName) ? "IsDeleted" : compareAttribute.IsDeletedFieldName, StringComparison.InvariantCultureIgnoreCase)).Key;
                if (_fieldManager.AreFieldsSame(oldValue, newValue, delField) == FieldCompareResult.NotEqual)
                    operationType = OperationType.Delete;
            }

            var newEvent = MakeEventObject(newValue, compareAttribute, comaprePropertyAttributes, operationType);
            var newEventSubject = MakeEventSubjectObject(objID.Value, newValue, compareAttribute, comaprePropertyAttributes, newEvent);

            foreach (var prop in comaprePropertyAttributes)
            {
                if (oldValue != null)
                {
                    var compareResult = _fieldManager.AreFieldsSame(oldValue, newValue, prop.Key);
                    switch (compareResult)
                    {
                        case FieldCompareResult.InvalidField:
                            return new BaseResult<Event, EventFaults>(null, EventFaults.ComparableAttrubutesNotSet);
                            break;
                        case FieldCompareResult.ObjectTypeDiffers:
                            return new BaseResult<Event, EventFaults>(null, EventFaults.ObjectTypeDiffers);
                            break;
                        case FieldCompareResult.Equal:
                            continue;
                            break;
                    }
                }

                paramList.AddRange(await createParams(prop.Value, prop.Key, oldValue, newValue, newEventSubject, formatter?.Invoke(prop.Value, prop.Key)));

            }

            paramList.ForEach(p => newEventSubject.EventSubjectParam.Add(p));
            newEvent.EventSubject.Add(newEventSubject);

            return new BaseResult<Event, EventFaults>(newEvent, null);
        }

        private Guid? GetObjectID(object newValue, EntityCompareAttribute compareAttribute)
        {
            if (newValue == null)
                return null;
            var propId = newValue.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .Where(x => x.Name.Equals((string.IsNullOrWhiteSpace(compareAttribute.IdentifierField) ? "id" : compareAttribute.IdentifierField), StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();
            if (propId == null)
                return null;
            if (propId.PropertyType == typeof(Guid))
                return (Guid)propId.GetValue(newValue);
            return null;
        }

        private EventSubject MakeEventSubjectObject(Guid objectId, object obj, EntityCompareAttribute entityAttribute, Dictionary<string, FieldCompareAttribute> fieldsAttributes, Event newEvent)
        {
            var result = new EventSubject()
            {
                ClassId = (ObjectClass?)entityAttribute.ClassID,
                Id = Guid.NewGuid(),
                ObjectId = objectId,
                SubjectName = entityAttribute.EntityName,
                SubjectValue = GenerateObjectName(obj, fieldsAttributes, true),
            };
            newEvent.EventSubject.Add(result);
            return result;
        }

        private Event MakeEventObject(object obj,EntityCompareAttribute entityAttribute, Dictionary<string, FieldCompareAttribute> fieldsAttributes, OperationType operationType)
        {
            StringBuilder message = new StringBuilder();
            int operId = 0;// isAdd ? entityAttribute.AddOperationID : entityAttribute.EditOperationID;
            switch(operationType)
            {
                case OperationType.Add:
                    message.Append(entityAttribute.AddActionLabel ?? "Добавлена");
                    operId = entityAttribute.AddOperationID;
                    break;
                case OperationType.Edit:
                    message.Append(entityAttribute.EditActionLabel ?? "Сохранена");
                    operId = entityAttribute.EditOperationID;
                    break;
                case OperationType.Delete:
                    message.Append(entityAttribute.DeleteActionLabel ?? "Удалена");
                    operId = entityAttribute.DeleteOperationID;
                    break;
            }

            message.Append(' ')
                .Append(entityAttribute.EntityName)
                .Append(": ")
                .Append(GenerateObjectName(obj, fieldsAttributes, false));
            var _event = new Event(
                message.ToString(),
                operId > 0 ? operId : null,
                _userContextProvider.GetUserContext().UserID);
            return _event;
        }

        private string GenerateObjectName(object obj, Dictionary<string, FieldCompareAttribute> fieldsAttributes, bool shortOnly)
        {
            var nameProps = fieldsAttributes.Where(x => x.Value.NameOrder != null);
            if (shortOnly)
                nameProps = nameProps.OrderBy(x => x.Value.NameOrder).Take(1);
            else
                nameProps = nameProps.OrderByDescending(x => x.Value.NameOrder);

            return string.Join(' ', nameProps.Select(x => _fieldManager.GetFieldValueLabel(obj, x.Key)));
        }

        private async Task<List<EventSubjectParam>> createParams(FieldCompareAttribute fieldAttribute, string fieldName, object oldValue, object newValue, EventSubject eventSubject, Func<object, Task<string>> formatter)
        {
            List<EventSubjectParam> result = new List<EventSubjectParam>();
            if (string.IsNullOrWhiteSpace(fieldAttribute.ListIDField))
            {
                var param = new EventSubjectParam(
                    fieldAttribute.UserFieldName,
                    oldValue == null ? fieldAttribute.NullValueLabel : (formatter == null ? _fieldManager.GetFieldValueLabel(oldValue, fieldName) : await formatter.Invoke(oldValue)),
                    formatter == null ? _fieldManager.GetFieldValueLabel(newValue, fieldName) : await formatter.Invoke(newValue));
                result.Add(param);
                eventSubject.EventSubjectParam.Add(param);
            }
            else
            {
                var oldField = _fieldManager.GetFieldValue(oldValue, fieldName);
                var newField = _fieldManager.GetFieldValue(newValue, fieldName);
                var oldList = oldField as System.Collections.IEnumerable;
                var newList = newField as System.Collections.IEnumerable;
                if ((oldList == null && oldField != null) || (newList == null && newField !=null))
                    throw new ArgumentOutOfRangeException($"{fieldName} is not {nameof(System.Collections.IEnumerable)} but has {nameof(fieldAttribute.ListIDField)} attribute set to {fieldAttribute.ListIDField}");
                if (oldList != null)
                {
                    foreach (var oldItem in oldList)
                    {
                        object sameItem = null;
                        if (newList != null)
                        {
                            foreach (var newItem in newList)
                            {
                                if (_fieldManager.GetFieldValue(oldItem, fieldAttribute.ListIDField).Equals(_fieldManager.GetFieldValue(newItem, fieldAttribute.ListIDField)))
                                {
                                    sameItem = newItem;
                                    break;
                                }
                            }
                        }
                        var prmOld = formatter == null ? oldItem?.ToString() : await formatter.Invoke(oldItem);
                        var prmNew = sameItem == null ? string.Empty : (formatter == null ? sameItem.ToString() : await formatter.Invoke(sameItem));
                        if (!prmOld.Equals(prmNew))
                        {
                            var param = new EventSubjectParam(fieldAttribute.UserFieldName, prmOld, prmNew);
                            result.Add(param);
                            eventSubject.EventSubjectParam.Add(param);
                        }
                    }
                }
                if (newList != null)
                {
                    foreach (var newItem in newList)
                    {
                        object sameItem = null;
                        if (oldList != null)
                        {
                            foreach (var oldItem in oldList)
                            {
                                if (_fieldManager.GetFieldValue(oldItem, fieldAttribute.ListIDField).Equals(_fieldManager.GetFieldValue(newItem, fieldAttribute.ListIDField)))
                                {
                                    sameItem = newItem;
                                    break;
                                }
                            }
                        }
                        if (sameItem == null)
                        {
                            var prmNew = formatter == null ? newItem.ToString() : await formatter.Invoke(newItem) ;
                            var param = new EventSubjectParam(fieldAttribute.UserFieldName, string.Empty, prmNew);
                            result.Add(param);
                            eventSubject.EventSubjectParam.Add(param);
                        }
                    }
                }
            }

            return result;
        }
    }
}

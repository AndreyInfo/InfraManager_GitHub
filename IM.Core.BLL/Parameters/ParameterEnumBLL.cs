using AutoMapper;
using InfraManager.BLL.Events;
using InfraManager.BLL.Parameters;
using InfraManager.DAL;
using InfraManager.DAL.Events;
using InfraManager.DAL.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DataStructures.Graphs;
using InfraManager.DAL.FormBuilder;
using InfraManager.BLL.ServiceDesk.CustomValues;
using Newtonsoft.Json;
using InfraManager.BLL.Localization;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.ParameterEnumBLL
{
    internal class ParameterEnumBLL : IParameterEnumBLL, ISelfRegisteredService<IParameterEnumBLL>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<ParameterEnum> _repository;
        private readonly IRepository<ParameterEnumValue> _parameterEnumValueRepository;
        private readonly IRepository<FormField> _formFieldsRepository;
        private readonly CustomValueFactory _customValueFactory;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IFinder<ParameterEnum> _finder;
        private readonly IFinder<ParameterEnumValue> _parameterEnumValueFinder;
        private readonly IEventBLL _eventBLL;
        private readonly IEventBuilder _eventMaker;
        private readonly ILocalizeText _localizeText;
        private readonly IGuidePaggingFacade<ParameterEnum, ParameterEnumListItem> _guidePaggingFacade;
        public ParameterEnumBLL(
            IMapper mapper,
            IRepository<ParameterEnum> repository,
            IRepository<ParameterEnumValue> parameterEnumValueRepository,
            IRepository<FormField> formFieldsRepository,
            CustomValueFactory customValueFactory,
            IFinder<ParameterEnum> finder,
            IFinder<ParameterEnumValue> parameterEnumValueFinder,
            IUnitOfWork saveChangesCommand,
            IEventBLL eventBLL,
            IEventBuilder eventMaker,
            ILocalizeText localizeText,
            IGuidePaggingFacade<ParameterEnum, ParameterEnumListItem> guidePaggingFacade)
        {
            _mapper = mapper;
            _repository = repository;
            _finder = finder;
            _parameterEnumValueRepository = parameterEnumValueRepository;
            _parameterEnumValueFinder = parameterEnumValueFinder;
            _formFieldsRepository = formFieldsRepository;
            _customValueFactory = customValueFactory;
            _saveChangesCommand = saveChangesCommand;
            _eventBLL = eventBLL;
            _eventMaker = eventMaker;
            _localizeText = localizeText;
            _guidePaggingFacade = guidePaggingFacade;
        }

        public async Task<ParameterEnumDetails[]> GetParameterEnumsAsync(ParameterEnumFilter filter, CancellationToken cancellationToken = default)
        {
            var query = _repository.Query();

            if (filter.IsTree.HasValue)
            {
                query = query.Where(x => x.IsTree == filter.IsTree);
            }

            var parameters = await _guidePaggingFacade.GetPaggingAsync(filter,
                query: query,
                x => x.Name.ToLower().Contains(filter.SearchString.ToLower()),
                cancellationToken: cancellationToken);

            return _mapper.Map<ParameterEnumDetails[]>(parameters);
        }

        public async Task<ParameterEnumDetails> GetParameterEnumAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var parameter = await _finder.FindAsync(id, cancellationToken);
            var result = _mapper.Map<ParameterEnumDetails>(parameter);
            return result;
        }

        private async Task SaveHistoryAsync(object newValue, object oldValue, CancellationToken cancellationToken = default)
        {
            var eventResult = await _eventMaker.CreateEvent(oldValue, newValue);
            if (!eventResult.Success)
                throw new Exception("Ошибка создания события истории");
            _eventBLL.AddEvent(eventResult.Result);
        }

        public async Task<ParameterEnumDetails> AddParameterEnumAsync(ParameterEnumDetails parameterData, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<ParameterEnum>(parameterData);
            _repository.Insert(entity);
            var newValue = _mapper.Map<ParameterEnumDetails>(entity);
            await SaveHistoryAsync(newValue, null, cancellationToken);
            await _saveChangesCommand.SaveAsync(cancellationToken);

            return _mapper.Map<ParameterEnumDetails>(entity);
        }

        public async Task<ParameterEnumDetails> UpdateParameterEnumAsync(ParameterEnumDetails parameterData, CancellationToken cancellationToken = default)
        {
            var foundEntity = await _finder.FindAsync(parameterData.ID, cancellationToken);
            if (foundEntity is null)
                throw new ObjectNotFoundException<Guid>(parameterData.ID, ObjectClass.ParameterEnum);

            var oldValue = _mapper.Map<ParameterEnumDetails>(foundEntity);
            await SaveHistoryAsync(parameterData, oldValue, cancellationToken);
            _mapper.Map(parameterData, foundEntity);
            await _saveChangesCommand.SaveAsync(cancellationToken);

            return _mapper.Map<ParameterEnumDetails>(foundEntity);
        }

        public async Task DeleteParameterEnumAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (await HasRelatedFormAsync(id, cancellationToken))
                throw new InvalidObjectException(await _localizeText.LocalizeAsync(nameof(Resources.CantDeleteRelatedDictionary), cancellationToken));

            var foundEntity = await _finder.FindAsync(id, cancellationToken);
            if (foundEntity.ParameterEnumValues.Any())
            {
                foreach (var entity in foundEntity.ParameterEnumValues)
                {
                    await DeleteParameterValueAsync(entity.ID, cancellationToken);
                }
            }
            _repository.Delete(foundEntity);
            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        public async Task<ParameterEnumValuesData[]> GetParameterEnumValuesAsync(Guid parameterEnumID, CancellationToken cancellationToken = default)
        {
            var parameterValues = await _parameterEnumValueRepository.ToArrayAsync(p => p.ParameterEnumID == parameterEnumID, cancellationToken);
            var isTree = parameterValues?.FirstOrDefault().ParameterEnum.IsTree;
            if (isTree == true)
            {
                parameterValues = parameterValues.Where(p => p.ParentID == null).ToArray();
            }
            var values = _mapper.Map<ParameterEnumValuesData[]>(parameterValues);
            return values;
        }

        public async Task<ParameterEnumValueData> GetParameterEnumValueAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var parameter = await _parameterEnumValueFinder.FindAsync(id, cancellationToken);
            var result = _mapper.Map<ParameterEnumValueData>(parameter);
            return result;
        }

        private async Task<ParameterEnumValueData> AddParameterEnumValueAsync(ParameterEnumValueData parameterData, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<ParameterEnumValue>(parameterData);
            _parameterEnumValueRepository.Insert(entity);
            var newValue = _mapper.Map<ParameterEnumValueData>(entity);
            await SaveHistoryAsync(newValue, null, cancellationToken);
            await _saveChangesCommand.SaveAsync(cancellationToken);
            return _mapper.Map<ParameterEnumValueData>(entity);
        }

        private async Task<ParameterEnumValueData> UpdateParameterEnumValueAsync(ParameterEnumValueData parameterData, CancellationToken cancellationToken = default)
        {
            var foundEntity = await _parameterEnumValueFinder.FindAsync(parameterData.ID, cancellationToken);
            if (foundEntity is null)
                throw new ObjectNotFoundException<Guid>(parameterData.ID, "ParameterEnumValue");

            var oldValue = _mapper.Map<ParameterEnumValueData>(foundEntity);
            await SaveHistoryAsync(parameterData, oldValue, cancellationToken);
            _mapper.Map(parameterData, foundEntity);
            await _saveChangesCommand.SaveAsync(cancellationToken);

            return _mapper.Map<ParameterEnumValueData>(foundEntity);
        }

        public async Task UpdateParameterEnumValuesAsync(List<ParameterEnumValuesData> parameterValuesData, CancellationToken cancellationToken = default)
        {
            foreach (var parameterValue in parameterValuesData)
            {
                await UpdateParameterEnumValueRecursiveAsync(parameterValue, cancellationToken);
            }
        }

        private async Task UpdateParameterEnumValueRecursiveAsync(ParameterEnumValuesData nodes, CancellationToken cancellationToken = default)
        {
            var treenode = new ParameterEnumValueData();
            if (nodes.Parent.ID != Guid.Empty)
            {
                await UpdateParameterEnumValueAsync(nodes.Parent, cancellationToken);
            }
            else
            {
                treenode = await AddParameterEnumValueAsync(nodes.Parent, cancellationToken);
            }
            if (nodes.Childrens != null)
            {
                foreach (var child in nodes.Childrens)
                {
                    if (child != null)
                    {
                        if (nodes.Parent.ID == Guid.Empty)
                        {
                            child.Parent.ParentID = treenode.ID;
                        }
                        await UpdateParameterEnumValueRecursiveAsync(child, cancellationToken);
                    }
                }
            }
        }

        private async Task DeleteParameterValueAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var foundEntity = await _parameterEnumValueFinder.FindAsync(id, cancellationToken);
            if (foundEntity != null)
            {
                if (foundEntity.ParameterEnums.Any())
                {
                    foreach (var child in foundEntity.ParameterEnums)
                    {
                        await DeleteParameterValueAsync(child.ID, cancellationToken);
                    }
                }
                _parameterEnumValueRepository.Delete(foundEntity);
            }
        }

        public async Task DeleteParameterEnumValueAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var foundEntity = await _parameterEnumValueFinder.FindAsync(id, cancellationToken);
            await DeleteParameterValueAsync(foundEntity.ID, cancellationToken);
            await _saveChangesCommand.SaveAsync(cancellationToken);
        }

        public async Task<Event[]> GetHistoryAsync(Guid id, DateTime? dateFrom, DateTime? dateTill, CancellationToken cancellationToken = default)
        {
            return await _eventBLL.GetEventsAsync(id, dateFrom, dateTill, cancellationToken);
        }

        private async Task<bool> HasRelatedFormAsync(Guid id, CancellationToken cancellationToken = default)
        {
            foreach (var item in await _formFieldsRepository
                .ToArrayAsync(x => x.Type == FieldTypes.EnumComboBox || x.Type == FieldTypes.EnumRadioButton, cancellationToken))
                if (JsonConvert.DeserializeObject<dynamic>(item.SpecialFields)["Source"] == id)
                    return true;

            return false;
        }
    }
}

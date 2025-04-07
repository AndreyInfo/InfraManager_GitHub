using AutoMapper;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contracts = InfraManager.CrossPlatform.WebApi.Contracts.SoftwareLicenceScheme.Models;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Attributes;
using InfraManager.BLL.FieldEdit;
using InfraManager.DAL.Software;
using InfraManager.BLL.Events;
using InfraManager.BLL.EntityFieldEditor;
using InfraManager.BLL.Extensions;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;

namespace InfraManager.BLL.Software
{
    [ObjectClassMapping(ObjectClass.LicenceScheme)]
    internal class SoftwareLicenceSchemeBLL : ISoftwareLicenceSchemeBLL, IEntityEditor, ISelfRegisteredService<ISoftwareLicenceSchemeBLL>
    {
        private readonly ISoftwareLicenceSchemeDataProvider _softwareLicenceSchemeDataProvider;
        private readonly IFinder<AdapterType> _assetsDataProvider;
        private readonly IMapper _mapper;
        private readonly IEventBLL _eventDataProvider;
        private readonly IEventBuilder _eventMaker;
        private readonly IFieldManager _fieldSetter;
        private readonly IUnitOfWork _saveChangesCommand;

        public SoftwareLicenceSchemeBLL(
            ISoftwareLicenceSchemeDataProvider softwareLicenceSchemeDataProvider,
            IEventBLL eventDataProvider,
            IEventBuilder eventMaker,
            IFieldManager fieldSetter,
            IFinder<AdapterType> assetsDataProvider,
            IMapper mapper,
            IUnitOfWork saveChangesCommand)
        {
            _softwareLicenceSchemeDataProvider = softwareLicenceSchemeDataProvider ?? throw new ArgumentNullException(nameof(softwareLicenceSchemeDataProvider));
            _assetsDataProvider = assetsDataProvider ?? throw new ArgumentNullException(nameof(assetsDataProvider));
            _eventDataProvider = eventDataProvider ?? throw new ArgumentNullException(nameof(eventDataProvider));
            _eventMaker = eventMaker ?? throw new ArgumentNullException(nameof(eventMaker));
            _fieldSetter = fieldSetter ?? throw new ArgumentNullException(nameof(fieldSetter));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _saveChangesCommand = saveChangesCommand;
        }

        public async Task<BaseResult<List<Contracts.SoftwareLicenceSchemeListItem>, BaseError>> GetListAsync(string searchText = null, bool showDeleted = false, SortModel sortModel = null, CancellationToken cancellationToken = default)
        {

            var enteties = await _softwareLicenceSchemeDataProvider.GetListAsync(searchText, showDeleted, cancellationToken);

            var mapedResult = enteties.Select(x => _mapper.Map<Contracts.SoftwareLicenceSchemeListItem>(x));
            if(sortModel!=null && typeof(Contracts.SoftwareLicenceSchemeListItem).GetProperty(sortModel.FieldName)!=null)
            {
                PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(Contracts.SoftwareLicenceSchemeListItem)).Find(sortModel.FieldName, true);
                mapedResult = (sortModel.IsDescending ? mapedResult.OrderByDescending(x => prop.GetValue(x)) : mapedResult.OrderBy(x => prop.GetValue(x)));
            }
            return new BaseResult<List<Contracts.SoftwareLicenceSchemeListItem>, BaseError>(mapedResult.ToList(), null);

        }

        public async Task<BaseResult<Contracts.SoftwareLicenceScheme, BaseError>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
                return new BaseResult<Contracts.SoftwareLicenceScheme, BaseError>(null, BaseError.BadParamsError);
            var scheme = _mapper.Map<Contracts.SoftwareLicenceScheme>(await _softwareLicenceSchemeDataProvider.GetAsync(id, cancellationToken));
            scheme.SoftwareLicenceSchemeCoefficients = _mapper.Map<Contracts.SoftwareLicenceSchemeCoefficient[]>(await _softwareLicenceSchemeDataProvider.GetCoefficientListAsync(id, cancellationToken));
            return new BaseResult<Contracts.SoftwareLicenceScheme, BaseError>(scheme, null);
        }

        public async Task<BaseResult<Guid, Contracts.SoftwareLicenceSchemeRules>> SaveAsync(Contracts.SoftwareLicenceScheme softwareLicenceScheme, CancellationToken cancellationToken = default)
        {
            var item = await _softwareLicenceSchemeDataProvider.GetAsync(softwareLicenceScheme.ID, cancellationToken);
            var coeffs = await _softwareLicenceSchemeDataProvider.GetCoefficientListAsync(softwareLicenceScheme.ID, cancellationToken);
            var result =  await SaveInternal(softwareLicenceScheme, item, coeffs, null, cancellationToken);
            await _saveChangesCommand.SaveAsync();
            return result;
        }

        public async Task<BaseResult<Contracts.SoftwareLicenceSchemeDeleteResponse, Contracts.SoftwareLicenceSchemeRules>> DeleteRestoreAsync(Contracts.SoftwareLicenceSchemeDeleteRequest request, bool doRestore, CancellationToken cancellationToken = default)
        {
            Contracts.SoftwareLicenceSchemeDeleteResponse result = new Contracts.SoftwareLicenceSchemeDeleteResponse();
            result.Results = new List<BaseResult<Guid, Contracts.SoftwareLicenceSchemeRules>>();
            
            foreach(var itm in request.Guids)
            {
                result.Results.Add(await handleLocal(itm, doRestore, cancellationToken));
            }
            await _saveChangesCommand.SaveAsync(cancellationToken);
            return new BaseResult<Contracts.SoftwareLicenceSchemeDeleteResponse, Contracts.SoftwareLicenceSchemeRules>(result, null);


            async Task<BaseResult<Guid, Contracts.SoftwareLicenceSchemeRules>> handleLocal(Guid id, bool doRestore, CancellationToken cancellationToken)
            {
                var item = await _softwareLicenceSchemeDataProvider.GetAsync(id, cancellationToken);
                if(item == null)
                    return new BaseResult<Guid, Contracts.SoftwareLicenceSchemeRules>(id, Contracts.SoftwareLicenceSchemeRules.SchemeNotFound);

                var oldValue = _mapper.Map<Contracts.SoftwareLicenceScheme>(item);
                var valid = Validate(oldValue);
                if (valid != null)
                    return new BaseResult<Guid, Contracts.SoftwareLicenceSchemeRules>(id, valid);
                var newValue = _mapper.Map<Contracts.SoftwareLicenceScheme>(item);
                if(newValue.IsDeleted == doRestore)
                {
                    newValue.IsDeleted = !doRestore;
                    item.UpdatedDate = DateTime.UtcNow;
                    item.IsDeleted = !doRestore;
                    var _event = await _eventMaker.CreateEvent(oldValue, newValue);
                    if (!_event.Success)
                        return new BaseResult<Guid, Contracts.SoftwareLicenceSchemeRules>(id, Contracts.SoftwareLicenceSchemeRules.EventFault);
                    _eventDataProvider.AddEvent(_event.Result);
                }

                return new BaseResult<Guid, Contracts.SoftwareLicenceSchemeRules>(id, null );
            }
        }

        public bool CanHandle(ObjectClassModel objectClass)
        {
            return objectClass.ObjClassID == 750; //need to make single list of object type lists
        }

        public async Task<BaseResult<SetFieldResult, BaseError>> HandleAsync(SetFieldRequest model, CancellationToken cancellationToken)
        {

             var dbItem = await _softwareLicenceSchemeDataProvider.GetAsync(model.ID, cancellationToken);
             var dbCoeffs = await _softwareLicenceSchemeDataProvider.GetCoefficientListAsync(model.ID, cancellationToken);

             var changeItem = _mapper.Map<Contracts.SoftwareLicenceScheme>(dbItem);
             changeItem.SoftwareLicenceSchemeCoefficients = _mapper.Map<Contracts.SoftwareLicenceSchemeCoefficient[]>(dbCoeffs);

             var setResult = _fieldSetter.SetFieldValue(changeItem, model.FieldValue);
             if (setResult != null)
                 return new BaseResult<SetFieldResult, BaseError>(null, setResult);

             var result = await SaveInternal(changeItem, dbItem, dbCoeffs, model.FieldValue.Field, cancellationToken);
             if (result.Success)
             {
                 return new BaseResult<SetFieldResult, BaseError>(new SetFieldResult(model) {CurrentObjectValue =  _fieldSetter.GetFieldValue(changeItem, model.FieldValue.Field)}, null);
             }
             await _saveChangesCommand.SaveAsync(cancellationToken);
             return new BaseResult<SetFieldResult, BaseError>(null, BaseError.BadParamsError);
        }

        #region private

        private Contracts.SoftwareLicenceSchemeRules? Validate(Contracts.SoftwareLicenceScheme softwareLicenceScheme, string updateFieldName = null)
        {
            if (string.IsNullOrWhiteSpace(softwareLicenceScheme.Name))
                return Contracts.SoftwareLicenceSchemeRules.NameObligatory;
            if (softwareLicenceScheme.SchemeType == Contracts.SoftwareLicenceSchemeType.System && !(updateFieldName??string.Empty).Equals(nameof(Contracts.SoftwareLicenceScheme.SoftwareLicenceSchemeCoefficients), StringComparison.InvariantCultureIgnoreCase))
                return Contracts.SoftwareLicenceSchemeRules.ModificationProhibited;
            return null;
        }

        private async Task<BaseResult<Guid, Contracts.SoftwareLicenceSchemeRules>> SaveInternal(Contracts.SoftwareLicenceScheme valueFromRequest, 
            SoftwareLicenceScheme valueFromDB = null, List<SoftwareLicenceSchemeProcessorCoeff> coeffsFromDB = null, 
            string updateFieldName = null,
            CancellationToken cancellationToken = default)
        {
            var valid = Validate(valueFromRequest, updateFieldName);
            if (valid != null)
                return new BaseResult<Guid, Contracts.SoftwareLicenceSchemeRules>(Guid.Empty, valid);

            bool isNew = valueFromDB == null;
            Contracts.SoftwareLicenceScheme oldValue = null;
            if (isNew)
            {
                if (valueFromRequest.ID == Guid.Empty)
                    valueFromRequest.ID = Guid.NewGuid();
                valueFromDB = _mapper.Map<SoftwareLicenceScheme>(valueFromRequest);
                coeffsFromDB = _mapper.Map<List<SoftwareLicenceSchemeProcessorCoeff>>(valueFromRequest.SoftwareLicenceSchemeCoefficients);
            }
            else
            {
                oldValue = _mapper.Map<Contracts.SoftwareLicenceScheme>(valueFromDB);
                oldValue.SoftwareLicenceSchemeCoefficients = coeffsFromDB == null ? null : _mapper.Map<Contracts.SoftwareLicenceSchemeCoefficient[]>(coeffsFromDB);
                valid = Validate(oldValue, updateFieldName);
                if (valid != null)
                    return new BaseResult<Guid, Contracts.SoftwareLicenceSchemeRules>(Guid.Empty, valid);
                _mapper.Map(valueFromRequest, valueFromDB, typeof(Contracts.SoftwareLicenceScheme), typeof(SoftwareLicenceScheme));
                if (coeffsFromDB != null)
                {
                    foreach (var c in coeffsFromDB)
                    {
                        var newValue = valueFromRequest.SoftwareLicenceSchemeCoefficients?.FirstOrDefault(x => x.ProcessorTypeID == c.ProcessorTypeId);
                        if (newValue != null)
                            c.Coefficient = newValue.Coefficient;
                        else
                            _softwareLicenceSchemeDataProvider.Delete(c);
                    };
                }
                if(valueFromRequest.SoftwareLicenceSchemeCoefficients!=null)
                {
                    foreach(var c in valueFromRequest.SoftwareLicenceSchemeCoefficients)
                    {
                        if(!(coeffsFromDB?.Any(x=>x.ProcessorTypeId == c.ProcessorTypeID) ?? false))
                        {
                            var newValue = _mapper.Map<SoftwareLicenceSchemeProcessorCoeff>(c);
                            newValue.LicenceSchemeId = valueFromRequest.ID;
                            await _softwareLicenceSchemeDataProvider.AddAsync(newValue);
                        }
                    }
                }
            }

            var check = await _softwareLicenceSchemeDataProvider.GetByNameAsync(valueFromRequest.Name, cancellationToken);
            if(isNew && check !=null)
                return new BaseResult<Guid, Contracts.SoftwareLicenceSchemeRules>(Guid.Empty, Contracts.SoftwareLicenceSchemeRules.UniqueName);
            if (!isNew && !(check == null || check.ID == valueFromRequest.ID))
                return new BaseResult<Guid, Contracts.SoftwareLicenceSchemeRules>(Guid.Empty, Contracts.SoftwareLicenceSchemeRules.UniqueName);

            valueFromDB.UpdatedDate = DateTime.UtcNow;

            if (isNew)
            {
                valueFromDB.CreatedDate = valueFromDB.UpdatedDate;
                await _softwareLicenceSchemeDataProvider.AddAsync(valueFromDB);
                if(coeffsFromDB?.Any() ?? false)
                    foreach(var c in coeffsFromDB)
                    {
                        await _softwareLicenceSchemeDataProvider.AddAsync(c);
                    }
            }
            var eventResult = await _eventMaker.CreateEvent(oldValue, valueFromRequest, nameResolveFactory);
            if (!eventResult.Success)
                return new BaseResult<Guid, Contracts.SoftwareLicenceSchemeRules>(Guid.Empty, Contracts.SoftwareLicenceSchemeRules.EventFault);
            _eventDataProvider.AddEvent(eventResult.Result);

            return new BaseResult<Guid, Contracts.SoftwareLicenceSchemeRules>(valueFromDB.ID, null);
        }

        private Func<object, Task<string>> nameResolveFactory(FieldCompareAttribute arg, string fieldName)
        {
            if (arg.ListIDField == nameof(Contracts.SoftwareLicenceSchemeCoefficient.ProcessorTypeID))
                return ProcessorTypeResolver;
            return null;
        }

        private async Task<string> ProcessorTypeResolver(object arg)
        {
            var coeff = arg as Contracts.SoftwareLicenceSchemeCoefficient;
            if(coeff!=null)
            {
                var getter = await _assetsDataProvider.FindAsync(coeff.ProcessorTypeID);
                return getter!=null ? $"{getter.Name} = {coeff.Coefficient}" : null;
            }
            return null;
        }


        #endregion

        public async Task<BaseResult<List<ListItem>, BaseError>> GetSchemeTypesAsync(CancellationToken cancellationToken)
        {
            return new BaseResult<List<ListItem>, BaseError>(Contracts.SoftwareLicenceSchemeType.System.GetEnumListItems(), null);
        }

        public async Task<BaseResult<List<ListItem>, BaseError>> GetLicenseObjectTypesAsync(CancellationToken cancellationToken)
        {
            return new BaseResult<List<ListItem>, BaseError>(Contracts.SoftwareLicenceSchemeObjectTypes.RealComputer.GetEnumListItems(), null);
        }

        public async Task<BaseResult<ExpressionStatementsModel, BaseError>> GetLicenceExpressionStatements(CancellationToken cancellationToken)
        {
            return new BaseResult<ExpressionStatementsModel, BaseError>(
                new ExpressionStatementsModel
                {
                    Functions = new []
                    {
                        new StatementDefinitionModel("MAX( , )", "FormulaFunction_Max"),
                        new StatementDefinitionModel("MIN( , )", "FormulaFunction_Min"),
                        new StatementDefinitionModel(SoftwareLicenceSchemeExpressionBuilderCreator.CoefFunction, "FormulaFunction_Coef")
                    },
                    Variables = new []
                    {
                        new StatementDefinitionModel(SoftwareLicenceSchemeExpressionBuilderCreator.CoresVariableName, "FormulaVariable_N_Cores"),
                        new StatementDefinitionModel(SoftwareLicenceSchemeExpressionBuilderCreator.CpuVariableName, "FormulaVariable_N_CPU")
                    },
                    Operators = new []
                    {
                        new StatementDefinitionModel("+", "FormulaOperator_Plus"),
                        new StatementDefinitionModel("-", "FormulaOperator_Minus"),
                        new StatementDefinitionModel("*", "FormulaOperator_Multiplier"),
                        new StatementDefinitionModel("/", "FormulaOperator_Division"),
                        new StatementDefinitionModel("%", "FormulaOperator_DivisionRemaining"),
                    }
                }, 
                null);
        }

        private static Dictionary<string, Func<ExpressionFieldValidator<SoftwareLicenceScheme, ILicenceExpressionParameter, int>>> ExpressionValidators =
            new Dictionary<string, Func<ExpressionFieldValidator<SoftwareLicenceScheme, ILicenceExpressionParameter, int>>>
            {
                { 
                    nameof(SoftwareLicenceScheme.LicenseCountPerObject), 
                    () => SoftwareLicenceSchemeExpressionBuilderCreator.CreateValidator(x => x.LicenseCountPerObject)
                },
                {
                    nameof(SoftwareLicenceScheme.AdditionalRights),
                    () => SoftwareLicenceSchemeExpressionBuilderCreator.CreateValidator(x => x.AdditionalRights)
                }
            };

        private static Contracts.ExpressionValidationResponse ConvertResponseContract(ExpressionFieldValidationResult result)
        {
            return new Contracts.ExpressionValidationResponse
            {
                IsSuccess = result.Result == BaseError.Success,
                MessageKey = result.MessageKey,
                MessageArguments = result.MessageArguments
            };
        }

        public async Task<Contracts.ExpressionValidationResponse> Validate(string fieldName, object value, CancellationToken token = default)
        {
            if (!ExpressionValidators.ContainsKey(fieldName))
            {
                return new Contracts.ExpressionValidationResponse
                {
                    IsSuccess = true
                };
            }

            return ConvertResponseContract(ExpressionValidators[fieldName]().Validate(value?.ToString()));
        }
    }
}

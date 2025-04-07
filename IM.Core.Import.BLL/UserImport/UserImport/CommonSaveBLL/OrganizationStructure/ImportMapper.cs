using System.Linq.Expressions;
using AutoMapper;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.UserImport.CommonSaveBLL.OrganizationStructure;
using InfraManager;

namespace IM.Core.Import.BLL.Import;

internal abstract class ImportMapper<TDetails, TEntity> : IImportMapper<TDetails, TEntity>
{
    private readonly IDetailsLogic<TDetails> _detailsLogic;

    protected ImportMapper(IDetailsLogic<TDetails> detailsLogic)
    {
        _detailsLogic = detailsLogic;
    }

    protected virtual void AdditionalUpdateInit(ImportData<TDetails, TEntity> importData, TEntity entity)
    {
        
    }

    protected virtual void AdditionalInit(IMapper mapper, TDetails detail, TEntity entity,
        Dictionary<TDetails, TEntity> mappingDictionary)
    {
        
    }
    
    protected abstract Func<TDetails?,TDetails?>? Recursion { get; }
    
    protected abstract Action<TEntity,TEntity?>? SetRecursion { get; }

    public IEnumerable<TEntity> CreateMap(ImportData<TDetails, TEntity> data, IEnumerable<TDetails> details)
    {
        var detailsEnumerable = details as IList<TDetails> ?? details.ToList();
        if (!detailsEnumerable.Any())
            yield break;
        var mappingDictionary = new Dictionary<TDetails, TEntity>();
        foreach (var detail in detailsEnumerable)
        {
            var mapper = GetMapper(data.ImportFields, detail);

            var current = detail;
            TEntity? currentMapped = default(TEntity?);
            TEntity? baseMapped = default(TEntity?);
            while (current!=null)
            {
                TEntity mapped;
                if (mappingDictionary.ContainsKey(current))
                    mapped = mappingDictionary[current];
                else
                {
                    mapped =  mapper.Map<TEntity>(current);
                    mappingDictionary[current] = mapped;
                }
                AdditionalInit(mapper, current, mapped, mappingDictionary);
                baseMapped ??= mapped;
                if (currentMapped != null)
                    SetRecursion?.Invoke(currentMapped, mapped);
                currentMapped = mapped;
                current = Recursion == null ? default(TDetails?) : Recursion(current);
            };
            yield return baseMapped;
        }
    }
    
    public void UpdateMap(ImportData<TDetails,TEntity> data, IEnumerable<(TDetails, TEntity)> updatePairs)
    {
        var mappingDictionary = new Dictionary<TDetails, TEntity>();
        foreach (var updatePair in updatePairs)
        {
            var mapper = GetMapper(data.ImportFields,updatePair.Item1);

            mapper.Map(updatePair.Item1, updatePair.Item2);
            mappingDictionary[updatePair.Item1] = updatePair.Item2;
            AdditionalInit(mapper,updatePair.Item1, updatePair.Item2, mappingDictionary);
            AdditionalUpdateInit(data, updatePair.Item2);
        }

    }
   
    protected abstract void SetIgnoreFields(ObjectType flags, IMappingExpression<TDetails, TEntity> map);

    protected void IgnoreFieldsIf<T>(ObjectType flags,
        ObjectType flag,
        IMappingExpression<TDetails, TEntity> expression,
        params Expression<Func<TEntity, T>>[] ignoreFuncs
    )
    {
        if (flags.HasFlag(flag)) return;
        
        foreach (var ignoreFunc in ignoreFuncs)
        {
            expression.ForMember(ignoreFunc, x => x.Ignore());
        }
    }

    protected void Ignore<T>(IMappingExpression<TDetails, TEntity> expression,
        params Expression<Func<TEntity, T>>[] ignoreFuncs)
    {
        foreach (var ignoreFunc in ignoreFuncs)
        {
            expression.ForMember(ignoreFunc, x => x.Ignore());

        }
    }
    
    private  void MappingExpression(ObjectType flags,
        IMapperConfigurationExpression expression)
    {
        var map = expression.CreateMap<TDetails, TEntity>();
        SetIgnoreFields(flags, map);
    }

    private Dictionary<ObjectType, IMapper> _mappers = new();

   

    private IMapper GetMapper(ObjectType flags, TDetails details)
    { 
        var excludedFields = _detailsLogic.GetExcludedFields(details);
        var resultFlags = flags & ~excludedFields;
        if (_mappers.ContainsKey(resultFlags))
            return _mappers[resultFlags];
        var configuration = new MapperConfiguration(x => MappingExpression(resultFlags, x));
        var mapper = new Mapper(configuration);
        _mappers[resultFlags] = mapper;
        return mapper;
    }
}
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.ProductCatalogue.Models;
using InfraManager.DAL;
using Inframanager.DAL.ProductCatalogue.Synonyms;

namespace InfraManager.BLL.ProductCatalogue.Synonyms;

internal class SynonymInsertEntityListener<TEntity>:ISynonymInsertEntityListener<TEntity>
    where TEntity:class
{
    private readonly IRepository<Synonym> _repository;
    private readonly IMapper _mapper; 

    public SynonymInsertEntityListener(IRepository<Synonym> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task EntityAddedAsync(ProductModelOutputDetails entity, CancellationToken token)
    {
        var synonym = _mapper.Map<Synonym>(entity);
        _repository.Insert(synonym);
    }

}
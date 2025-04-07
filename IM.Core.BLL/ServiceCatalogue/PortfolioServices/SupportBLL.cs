using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using AutoMapper;
using InfraManager.DAL;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices;

internal class SupportBLL<T> : ISupportBLL<T>
                              where T : class
{
    private readonly IReadonlyRepository<T> _finder;
    private readonly IMapper _mapper;
    public SupportBLL(IReadonlyRepository<T> finder, IMapper mapper)
    {
        _finder = finder;
        _mapper = mapper;
    }


    public async Task<SupportLineResponsibleDetails> GetSupportByIdAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
    {
        var entity = await _finder.FirstOrDefaultAsync(expression, cancellationToken);
        if (entity is null)
            return null;

        return _mapper.Map<SupportLineResponsibleDetails>(entity);
    }
}

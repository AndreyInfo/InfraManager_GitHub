using AutoMapper;
using InfraManager.BLL.KnowledgeBase;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.KB
{
    internal class KnowledgeBaseAccessBLL : IKnowledgeBaseAccessBLL, ISelfRegisteredService<IKnowledgeBaseAccessBLL>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<KBArticleAccessList> _kBArticleAccessListRepository;
        private readonly IUnitOfWork _context;
        private readonly IReadonlyRepository<KBArticleAccessList> _readOnlykBArticleAccessListRepository;
        private readonly IGuidePaggingFacade<KBArticleAccessList, KBArticleAccessListModel> _paggingFacade;

        public KnowledgeBaseAccessBLL(
            IMapper mapper,
            IRepository<KBArticleAccessList> kBArticleAccessListRepository,
            IUnitOfWork context,
            IReadonlyRepository<KBArticleAccessList> readOnlykBArticleAccessListRepository,
            IGuidePaggingFacade<KBArticleAccessList, KBArticleAccessListModel> paggingFacade)
        {
            _mapper = mapper;
            _kBArticleAccessListRepository = kBArticleAccessListRepository;
            _context = context;
            _readOnlykBArticleAccessListRepository = readOnlykBArticleAccessListRepository;
            _paggingFacade = paggingFacade;
        }

        public async Task AddAsync(KBArticleAccessListModel accessListModel, CancellationToken cancellationToken)
        {
            if (!await _kBArticleAccessListRepository.AnyAsync(a => a.KbArticleID == accessListModel.KbArticleID && a.ObjectID == accessListModel.ObjectID, cancellationToken))
            {
                var accessListItem = _mapper.Map<KBArticleAccessListModel, KBArticleAccessList>(accessListModel);
                _kBArticleAccessListRepository.Insert(accessListItem);
                await _context.SaveAsync(cancellationToken);
            }
        }

        public async Task DeleteAsync(KBArticleAccessListModel accessListModel, CancellationToken cancellationToken)
        {
            var itemToDelete = await _kBArticleAccessListRepository.FirstOrDefaultAsync(f => f.KbArticleID == accessListModel.KbArticleID && f.ObjectID == accessListModel.ObjectID, cancellationToken);
            if (itemToDelete is not null)
            {
                _kBArticleAccessListRepository.Delete(itemToDelete);
                await _context.SaveAsync(cancellationToken);
            }
        }

        public async Task<KBArticleAccessListModel[]> GetListAsync(KBArticleAccessListFilter filter, CancellationToken cancellationToken)
        {
            var query = _readOnlykBArticleAccessListRepository.Query();

            if (filter.KbArticleID.HasValue)
            {
                query = query.Where(x => x.KbArticleID == filter.KbArticleID);
            }

            var accessList = await _paggingFacade.GetPaggingAsync(
                    filter,
                    query: query,
                    cancellationToken: cancellationToken);

            return _mapper.Map<KBArticleAccessListModel[]>(accessList);
        }
    }
}

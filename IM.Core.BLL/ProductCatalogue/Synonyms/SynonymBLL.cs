using System;
using Inframanager.BLL;
using InfraManager.DAL;
using Inframanager.DAL.ProductCatalogue.Synonyms;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ProductCatalogue.Synonyms
{
    internal class SynonymBLL :
        StandardBLL<Guid, Synonym, SynonymDetails, SynonymOutputDetails, SynonymFilter>,
        ISynonymBLL, ISelfRegisteredService<ISynonymBLL> 
    {
        public SynonymBLL(IRepository<Synonym> repository,
            ILogger<SynonymBLL> logger,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IBuildObject<SynonymOutputDetails, Synonym> detailsBuilder,
            IInsertEntityBLL<Synonym, SynonymDetails> insertEntityBLL,
            IModifyEntityBLL<Guid, Synonym, SynonymDetails, SynonymOutputDetails>
                modifyEntityBLL,
            IRemoveEntityBLL<Guid, Synonym> removeEntityBLL,
            IGetEntityBLL<Guid, Synonym, SynonymOutputDetails> detailsBLL,
            IGetEntityArrayBLL<Guid, Synonym, SynonymOutputDetails, SynonymFilter>
                detailsArrayBLL) : base(repository,
            logger,
            unitOfWork,
            currentUser,
            detailsBuilder,
            insertEntityBLL,
            modifyEntityBLL,
            removeEntityBLL,
            detailsBLL,
            detailsArrayBLL)
        {
        }
    }
}
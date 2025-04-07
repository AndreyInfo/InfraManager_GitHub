using System.Threading.Tasks;
using InfraManager.DAL;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System.Threading;

namespace InfraManager.BLL
{
    internal class ClassIconBLL : IClassIconBLL, ISelfRegisteredService<IClassIconBLL>
    {
        private readonly IReadonlyRepository<ClassIcon> _readOnlyRepository;
        private readonly IRepository<ClassIcon> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ClassIconBLL(IReadonlyRepository<ClassIcon> readOnlyRepository,
                            IRepository<ClassIcon> repository, 
                            IUnitOfWork unitOfWork)
        {
            _readOnlyRepository = readOnlyRepository;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ClassIcon> GetByIdAsync(ObjectClass classID, CancellationToken cancellationToken = default)
        {
            return await _readOnlyRepository.FirstOrDefaultAsync(x => x.ClassID == classID, cancellationToken);
        }

        public async Task<bool> AddAsync(ClassIcon[] models, CancellationToken cancellationToken = default)
        {
            foreach (var item in models)
                _repository.Insert(item);

            await _unitOfWork.SaveAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteAsync(ClassIcon[] models, CancellationToken cancellationToken = default)
        {
            foreach (var model in models)
                _repository.Delete(model);

            await _unitOfWork.SaveAsync(cancellationToken);
            return true;
        }

        public async Task<string> GetIconNameByClassIDAsync(ObjectClass classID, CancellationToken cancellationToken = default)
        {
            var result = await _readOnlyRepository.FirstOrDefaultAsync(x => x.ClassID == classID, cancellationToken);
            return result?.IconName;
        }

        public string GetIconNameByClassID(ObjectClass classID)
        {
            return GetIconNameByClassIDAsync(classID).Result;
        }
    }
}

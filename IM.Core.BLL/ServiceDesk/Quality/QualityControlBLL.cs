using AutoMapper;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.Quality;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Quality
{
    internal class QualityControlBLL : IQualityControlBLL, ISelfRegisteredService<IQualityControlBLL>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInsertEntityBLL<QualityControl, QualityControlData> _insertEntityBLL;
        private readonly IReadonlyRepository<QualityControl> _repository;

        public QualityControlBLL(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IInsertEntityBLL<QualityControl, QualityControlData> insertEntityBLL,
            IReadonlyRepository<QualityControl> repository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _insertEntityBLL = insertEntityBLL;
            _repository = repository;
        }

        public async Task<QualityControlDetails> AddAsync(QualityControlData data, CancellationToken cancellationToken = default)
        {
            var entity = await _insertEntityBLL.CreateAsync(data, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
            return _mapper.Map<QualityControlDetails>(entity);
        }

        public async Task<DateTime?> GetLastByCallAsync(Guid callID, CancellationToken cancellationToken = default)
        {
            var last = await _repository.Query().OrderByDescending(x => x.UtcDate).Where(x => x.CallID == callID).FirstOrDefaultAsync(cancellationToken); //TODO убрать использование EF Core
            return last?.UtcDate;
        }
    }
}

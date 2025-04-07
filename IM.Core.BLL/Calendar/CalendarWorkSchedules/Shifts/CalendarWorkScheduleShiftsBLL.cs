using AutoMapper;
using System;
using System.Threading.Tasks;
using System.Threading;
using InfraManager.DAL.CalendarWorkSchedules;
using InfraManager.DAL;
using InfraManager.ResourcesArea;
using InfraManager.BLL.Localization;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules.Shifts
{
    internal class CalendarWorkScheduleShiftsBLL : ICalendarWorkScheduleShiftsBLL
        , ISelfRegisteredService<ICalendarWorkScheduleShiftsBLL>
    {
        private readonly IRepository<CalendarWorkScheduleShift> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizeText _localize;

        public CalendarWorkScheduleShiftsBLL(IMapper mapper,
            IRepository<CalendarWorkScheduleShift> repository,
            IUnitOfWork unitOfWork,
            ILocalizeText localize)
        {
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _localize = localize;
        }

        public async Task<CalendarWorkScheduleShiftDetails[]> GetShiftsByIDAsync(Guid calendarWorkScheduleID, CancellationToken cancellationToken = default)
        {
            var entities = await _repository.ToArrayAsync(c => c.CalendarWorkScheduleID == calendarWorkScheduleID, cancellationToken);
            return _mapper.Map<CalendarWorkScheduleShiftDetails[]>(entities);
        }

        public async Task<Guid?> AddAsync(CalendarWorkScheduleShiftCreateData calendarWorkScheduleShiftDetails, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<CalendarWorkScheduleShift>(calendarWorkScheduleShiftDetails);
            _repository.Insert(entity);
            await _unitOfWork.SaveAsync(cancellationToken);
            return entity.ID;
        }

        public async Task<CalendarWorkScheduleShiftDetails> GetByIDAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(id, $"Not found {nameof(CalendarWorkScheduleShift)}");

            return _mapper.Map<CalendarWorkScheduleShiftDetails>(entity);
        }

        public async Task RemoveAsync(Guid id, CancellationToken cancelationToken = default)
        {
            var shift = await _repository.WithMany(c => c.WorkScheduleShiftExclusions)
                .FirstOrDefaultAsync(c => c.ID == id, cancelationToken)
                ?? throw new ObjectNotFoundException<Guid>(id, $"Not found {nameof(CalendarWorkScheduleShift)}");

            _repository.Delete(shift);

            await _unitOfWork.SaveAsync(cancelationToken);
        }

        public async Task UpdateAsync(CalendarWorkScheduleShiftDetails calendarWorkScheduleShiftDetails, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.FirstOrDefaultAsync(c => c.ID == calendarWorkScheduleShiftDetails.ID, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(calendarWorkScheduleShiftDetails.ID, $"Not found {nameof(CalendarWorkScheduleShift)}");

            _mapper.Map(calendarWorkScheduleShiftDetails, entity);

            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System.Threading;
using InfraManager.DAL.CalendarWorkSchedules;
using InfraManager.DAL;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using Inframanager;
using Microsoft.Extensions.Logging;
using InfraManager.BLL.ColumnMapper;
using InfraManager.BLL.Settings;
using InfraManager.BLL.Localization;
using InfraManager.ResourcesArea;
using InfraManager.BLL.Calendar.CalendarWorkSchedules.Items;
using InfraManager;
using InfraManager.BLL.CalendarService;
using InfraManager.BLL.Calendar.CalendarHolidays;
using InfraManager.BLL.Calendar.CalendarWorkSchedules.Shifts.Exclusions;

namespace InfraManager.BLL.Calendar.CalendarWorkSchedules
{
    internal sealed class CalendarWorkScheduleBLL : ICalendarWorkScheduleBLL
        , ISelfRegisteredService<ICalendarWorkScheduleBLL>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _saveChangesCommand;
        private readonly IRepository<CalendarWorkSchedule> _calendarWorkScheduleRepository;
        private readonly IReadonlyRepository<CalendarWeekend> _repositoryCalendarWeekends;
        private readonly IRepository<CalendarWorkScheduleItem> _calendarWorkScheduleItems;
        private readonly IReadonlyRepository<CalendarWorkScheduleDefault> _calendarWorkScheduleDefaultsRepository;
        private readonly IGuidePaggingFacade<CalendarWorkSchedule, CalendarSchedule> _guidePaggingFacade;
        private readonly IValidatePermissions<CalendarWorkSchedule> _validatePermissions;
        private readonly ILogger<CalendarWorkScheduleBLL> _logger;
        private readonly ICurrentUser _currentUser;
        private readonly ICalendarWorkScheduleItemBLL _calendarWorkScheduleItemBLL;
        private readonly ICalendarHolidayItemBLL _calendarHolidayItemBLL;

        private readonly IUserColumnSettingsBLL _columnBLL;
        private readonly IColumnMapper<CalendarWorkScheduleItem, DaysForTable> _columnMapper;
        private readonly ILocalizeText _localizeText;
        public CalendarWorkScheduleBLL(IMapper mapper,
                                       IUnitOfWork saveChangesCommand,
                                       IGuidePaggingFacade<CalendarWorkSchedule, CalendarSchedule> guidePaggingFacade,
                                       IRepository<CalendarWorkSchedule> calendarWorkScheduleRepository,
                                       IReadonlyRepository<CalendarWeekend> repositoryCalendarWeekends,
                                       IRepository<CalendarWorkScheduleItem> calendarWorkScheduleItems,
                                       IValidatePermissions<CalendarWorkSchedule> validatePermissions,
                                       ILogger<CalendarWorkScheduleBLL> logger,
                                       ICurrentUser currentUser,
                                       ICalendarWorkScheduleItemBLL calendarWorkScheduleItemBLL,
                                       ICalendarHolidayItemBLL calendarHolidayItemBLL,
                                       IUserColumnSettingsBLL columnBLL,
                                       IColumnMapper<CalendarWorkScheduleItem, DaysForTable> columnMapper,
                                       ILocalizeText localizeText,
                                       IReadonlyRepository<CalendarWorkScheduleDefault> calendarWorkScheduleDefaultsRepository)
        {
            _mapper = mapper;
            _saveChangesCommand = saveChangesCommand;
            _guidePaggingFacade = guidePaggingFacade;
            _calendarWorkScheduleRepository = calendarWorkScheduleRepository;
            _repositoryCalendarWeekends = repositoryCalendarWeekends;
            _calendarWorkScheduleItems = calendarWorkScheduleItems;
            _calendarHolidayItemBLL = calendarHolidayItemBLL;
            _validatePermissions = validatePermissions;
            _logger = logger;
            _currentUser = currentUser;
            _calendarWorkScheduleItemBLL = calendarWorkScheduleItemBLL;
            _columnMapper = columnMapper;
            _columnBLL = columnBLL;
            _localizeText = localizeText;
            _calendarWorkScheduleDefaultsRepository = calendarWorkScheduleDefaultsRepository;
        }

        #region Get

        /// <summary>
        /// Список
        /// </summary>
        /// <returns></returns>
        public async Task<CalendarWorkScheduleDetails[]> GetAsync(BaseFilter filter,
             CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetailsArray, cancellationToken);

            var result = await _guidePaggingFacade.GetPaggingAsync(
                filter,
                _calendarWorkScheduleRepository.Query(),
                c => c.Name.Contains(filter.SearchString),
                cancellationToken
            );

            return _mapper.Map<CalendarWorkScheduleDetails[]>(result);
        }

        public async Task<CalendarWorkScheduleWithRelatedDetails> GetByIDAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails, cancellationToken);
            var calendarEntity = await FindByIDAsync(guid, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(guid, ObjectClass.CalendarWorkSchedule);

            return _mapper.Map<CalendarWorkScheduleWithRelatedDetails>(calendarEntity);
        }


        private async Task<CalendarWorkSchedule> FindByIDAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _calendarWorkScheduleRepository
                .WithMany(x => x.WorkScheduleShifts)
                    .ThenWithMany(x => x.WorkScheduleShiftExclusions)
                .WithMany(x => x.WorkScheduleItems)
                .FirstOrDefaultAsync(x => x.ID == id, cancellationToken);
        }

        public async Task<CalendarWorkScheduleItemDetails[]> GetDaysByIDAsync(BaseFilter filter, Guid calendarWorkScheduleID, DataCalculateWorkSheduleDays modelDataCalculating, CancellationToken cancellationToken = default)
        {
            var calendar = await FindByIDAsync(calendarWorkScheduleID, cancellationToken);
            if (!calendar.WorkScheduleItems.Any())
                throw new Exception("WorkScheduleItems = 0");
            
            CalendarWorkScheduleItemDetails[] result;
            if (modelDataCalculating.ShiftTemplateLeft == default && string.IsNullOrEmpty(modelDataCalculating.ShiftTemplate))
                result = await _calendarWorkScheduleItemBLL.GetByFilterAsync(filter, calendarWorkScheduleID, cancellationToken);
            else
            {
                var days = await ExecuteGetDaysAsync(calendar, modelDataCalculating, filter, cancellationToken);
                result = _mapper.Map<CalendarWorkScheduleItemDetails[]>(days);
            }

            InitAdditionalParametr(result, calendar);

            return result;
        }

        private void InitAdditionalParametr(CalendarWorkScheduleItemDetails[] array, CalendarWorkSchedule calendar)
        {
            foreach (var item in array)
            {
                if (!item.ShiftNumber.HasValue && !item.DayType.IsWorkDay())
                {
                    item.TimeStart = null;
                    item.TimeEnd = null;
                    continue;
                }

                var shift = calendar.WorkScheduleShifts.FirstOrDefault(c => c.Number == item.ShiftNumber);
                item.TotalTimeSpanExclusionInMinutes = shift.WorkScheduleShiftExclusions.Sum(c => c.TimeSpanInMinutes);
            }
        }

        private async Task<CalendarWorkScheduleItem[]> ExecuteGetDaysAsync(CalendarWorkSchedule calendar, DataCalculateWorkSheduleDays dataForCalculating, BaseFilter filter, CancellationToken cancellationToken)
        {
            await RecalculteDaysAsync(calendar, dataForCalculating, cancellationToken);
            return await FilterDaysAsync(calendar.WorkScheduleItems, filter, cancellationToken);

        }
        #region  Пересчет
        private async Task<CalendarWorkScheduleItem[]> RecalculteDaysAsync(CalendarWorkSchedule calendar, DataCalculateWorkSheduleDays dataCalculating, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(dataCalculating.ShiftTemplate))
            {
                await ThrowIfHasNotCorrectSymbol(dataCalculating.ShiftTemplate, cancellationToken);
                await ThrowIfHasNotExistsShiftNumbers(dataCalculating.ShiftTemplate, calendar.WorkScheduleShifts, cancellationToken);

                await ChangeOrderDaysAsync(calendar, dataCalculating, cancellationToken);
            }

            return ReCalculateDays(calendar, dataCalculating.ShiftTemplateLeft);
        }

        private async Task ThrowIfHasNotCorrectSymbol(string shiftTemplate, CancellationToken cancellationToken)
        {
            var isCorrectTemplate = shiftTemplate.All(c => char.IsDigit(c) || IsSymbolWeekendByTemplate(c));
            if (!isCorrectTemplate)
                throw new InvalidObjectException(
                    await _localizeText.LocalizeAsync(nameof(Resources.ErrorNoCorrectTemplate), cancellationToken));
        }
        private async Task ThrowIfHasNotExistsShiftNumbers(string shiftTemplate, IEnumerable<CalendarWorkScheduleShift> workScheduleShifts, CancellationToken cancellationToken)
        {
            var shiftNumbersForRecalculate = shiftTemplate.Where(c => char.IsDigit(c)).Select(c => byte.Parse(c.ToString()));
            var shiftNumbersAlreadyExists = workScheduleShifts.Select(c => c.Number);
            var isContainsNotExistsShift = shiftNumbersForRecalculate.All(c => shiftNumbersAlreadyExists.Contains(c));
            if (!isContainsNotExistsShift)
                throw new InvalidObjectException(
                    await _localizeText.LocalizeAsync(nameof(Resources.ErrorInpitNoExsitsCalendarWorkScheduleShifts), cancellationToken));
        }

        private async Task<CalendarWorkSchedule> ChangeOrderDaysAsync(CalendarWorkSchedule calendar, DataCalculateWorkSheduleDays dataCalculating, CancellationToken cancellationToken = default)
        {
            var holidayDays = dataCalculating.CalendarHolidayID.HasValue
                                    ? await GetHolidayDatesAsync(dataCalculating.CalendarHolidayID.Value, calendar.Year, cancellationToken)
                                    : Array.Empty<DateTime>();

            var weekeneds = dataCalculating.CalendarWeekendID.HasValue
                                    ? await GetDayofWeekIsWeekenedAsync(dataCalculating.CalendarWeekendID.Value, cancellationToken)
                                    : Array.Empty<DayOfWeek>();


            CalculateWorkScheduleItems(calendar, holidayDays, weekeneds, dataCalculating.ShiftTemplate);

            return calendar;
        }

        private void CalculateWorkScheduleItems(in CalendarWorkSchedule calendar, DateTime[] holidays, DayOfWeek[] weekends, string shiftTemplate)
        {
            int index = 0;

            foreach (var item in calendar.WorkScheduleItems.OrderBy(c => c.DayOfYear))
            {
                if (index == shiftTemplate.Length)
                    index = 0;

                item.ShiftNumber = null;
                if (holidays.Select(c => c.DayOfYear).Contains(item.DayOfYear))
                    item.DayType = CalendarDayType.Holiday;
                else if (weekends.Contains(new DateTime(calendar.Year, 1, 1).AddDays(item.DayOfYear - 1).DayOfWeek))
                    item.DayType = CalendarDayType.Weekend;
                else if (IsSymbolWeekendByTemplate(shiftTemplate[index]))
                    item.DayType = CalendarDayType.WeekendByTemplate;
                else
                {
                    if (byte.TryParse(shiftTemplate[index].ToString(), out byte number))
                        ChangeParametersCalendarWorkScheduleForWorkDay(item, calendar.WorkScheduleShifts, number);
                    else
                        throw new Exception("Некоректный шаблон");
                }

                index++;
            }
        }

        /// <summary>
        /// Проверяет является ли символ введеной строки обозначением Выходного дня в шаблоне
        /// </summary>
        /// <param name="symbol">символ из шаблона</param>
        /// <returns></returns>
        private bool IsSymbolWeekendByTemplate(in char symbol)
        {
            var isEnglishSymbolHoliday = symbol.Equals('B');
            var isRussianSymbolHoliday = symbol.Equals('в') || symbol.Equals('В');
            return isRussianSymbolHoliday || isEnglishSymbolHoliday;
        }

        private async Task<DateTime[]> GetHolidayDatesAsync(Guid calendarHolidayID, int year, CancellationToken cancellationToken)
        {
            var holidayDays = await _calendarHolidayItemBLL.GetListAsync( calendarHolidayID, cancellationToken: cancellationToken);
            return holidayDays.Select(c => new DateTime(year, (int)c.Month, c.Day)).ToArray();
        }

        private async Task<DayOfWeek[]> GetDayofWeekIsWeekenedAsync(Guid calendarWeekendID, CancellationToken cancellationToken)
        {
            var calendarWeekend = await _repositoryCalendarWeekends.FirstOrDefaultAsync(c => c.ID == calendarWeekendID, cancellationToken);

            var result = new Queue<DayOfWeek>();
            if (calendarWeekend.Monday)
                result.Enqueue(DayOfWeek.Monday);
            if (calendarWeekend.Tuesday)
                result.Enqueue(DayOfWeek.Tuesday);
            if (calendarWeekend.Wednesday)
                result.Enqueue(DayOfWeek.Wednesday);
            if (calendarWeekend.Thursday)
                result.Enqueue(DayOfWeek.Thursday);
            if (calendarWeekend.Friday)
                result.Enqueue(DayOfWeek.Friday);
            if (calendarWeekend.Saturday)
                result.Enqueue(DayOfWeek.Saturday);
            if (calendarWeekend.Sunday)
                result.Enqueue(DayOfWeek.Sunday);

            return result.ToArray();
        }


        private void ChangeParametersCalendarWorkScheduleForWorkDay(in CalendarWorkScheduleItem item, in IEnumerable<CalendarWorkScheduleShift> calendarWorkScheduleShifts, byte shiftNumber)
        {
            item.ShiftNumber = shiftNumber;
            item.DayType = CalendarDayType.Work;

            var shift = calendarWorkScheduleShifts.FirstOrDefault(c => c.Number == shiftNumber);
            if (shift is null)
                return;

            item.TimeStart = shift.TimeStart;
            item.TimeSpanInMinutes = shift.TimeSpanInMinutes;
        }

        private CalendarWorkScheduleItem[] ReCalculateDays(CalendarWorkSchedule calendar, int shiftTemplateLeft)
        {
            var days = GetDaysByShifts(calendar, shiftTemplateLeft);

            calendar.WorkScheduleItems.Clear();
            days.ForEach(c => calendar.WorkScheduleItems.Add(c));

            return calendar.WorkScheduleItems.ToArray();
        }

        private CalendarWorkScheduleItem[] GetDaysByShifts(CalendarWorkSchedule calendar, int? shiftTemplateLeft)
        {
            var shifts = calendar.WorkScheduleItems.OrderBy(k => k.DayOfYear).ToArray();
            if (calendar.ShiftTemplateLeft > 0 && !shiftTemplateLeft.HasValue)
                shiftTemplateLeft = calendar.ShiftTemplateLeft;

            shifts = ShiftIndices(shifts, shiftTemplateLeft.Value);

            int daysCount = DateTime.IsLeapYear(calendar.Year) ? 366 : 365;
            int countOfCycles = shifts.Length;


            return Enumerable.Range(1, daysCount).Select(day =>
            {
                var shift = shifts[(day - 1) % countOfCycles];

                return new CalendarWorkScheduleItem()
                {
                    CalendarWorkSchedule = calendar,
                    ShiftNumber = shift.ShiftNumber,
                    CalendarWorkScheduleID = calendar.ID,
                    DayOfYear = (short)day,
                    DayType = shift.DayType,
                    TimeSpanInMinutes = shift.TimeSpanInMinutes,
                    TimeStart = shift.TimeStart
                };
            }).ToArray();
        }

        private static T[] ShiftIndices<T>(T[] array, int shift)
        {
            T[] shiftedArray = new T[array.Length];

            for (int i = shift; i < array.Length + shift; i++)
            {
                int shiftableIndex = i >= array.Length ? i - array.Length : i;
                shiftedArray[i - shift] = array[shiftableIndex];
            }

            return shiftedArray;
        }
        #endregion
        private async Task<CalendarWorkScheduleItem[]> FilterDaysAsync(IEnumerable<CalendarWorkScheduleItem> days, BaseFilter filter, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(filter.SearchString))
            {
                days = days.Where(c => c.DayOfYear.ToString().Equals(filter.SearchString));
            }

            var columns = await _columnBLL.GetAsync(_currentUser.UserId, filter.ViewName, cancellationToken);
            var orderColumn = columns.GetSortColumn();
            orderColumn.PropertyName = _columnMapper.MapFirst(orderColumn.PropertyName);

            //Сортировка происходит на уровне BLL, т.к. происходит пересчет на уровне на уровне BLL
            System.Reflection.PropertyInfo prop = typeof(CalendarWorkScheduleItem).GetProperty(orderColumn.PropertyName);
            days = (orderColumn.Ascending ? days.OrderBy(c => prop.GetValue(c, null))
                                         : days.OrderByDescending(c => prop.GetValue(c, null)))
                                         .Skip(filter.StartRecordIndex).Take(filter.CountRecords);

            return days.ToArray();
        }
        #endregion

        #region other
        public async Task<Guid> UpdateAsync(Guid id, CalendarWorkScheduleData data, CancellationToken cancelationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Update, cancelationToken);

            var existingEntity = await _calendarWorkScheduleRepository.FirstOrDefaultAsync(x => x.ID == id, cancelationToken)
                ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.CalendarWorkSchedule);
            _mapper.Map(data, existingEntity);

            await RecalculteDaysAsync(existingEntity
                                  ,_mapper.Map<DataCalculateWorkSheduleDays>(data)
                                  , cancelationToken);

            await _saveChangesCommand.SaveAsync(cancelationToken);

            return existingEntity.ID;
        }

        public async Task RemoveAsync(Guid id, CancellationToken cancelationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete, cancelationToken);

            _logger.LogTrace($"UserID = {_currentUser.UserId} start {ObjectAction.Delete} default {nameof(CalendarWorkSchedule)}");

            var entity = await FindByIDAsync(id, cancelationToken)
                ?? throw new ObjectNotFoundException<Guid>(id, ObjectClass.CalendarWorkSchedule);

            _calendarWorkScheduleRepository.Delete(entity);

            await _saveChangesCommand.SaveAsync(cancelationToken);

            _logger.LogTrace($"UserID = {_currentUser.UserId} finish {ObjectAction.Delete} default {nameof(CalendarWorkSchedule)}");
        }

        public async Task<Guid> CreateAsync(CalendarWorkScheduleData calendarWorkScheduleDetails, CancellationToken cancelationToken)
        {
            return await AddAsync(calendarWorkScheduleDetails, ObjectAction.Insert, cancelationToken);
        }

        public async Task<Guid> CreateByAnalogyAsync(CalendarWorkScheduleData calendarWorkScheduleDetails, CancellationToken cancelationToken)
        {
            return await AddAsync(calendarWorkScheduleDetails, ObjectAction.InsertAs, cancelationToken);
        }

        public async Task<CalendarWorkScheduleItemExclusionDetails[]> GetExclusionsAsync(Guid workScheduleID, int? dayOfYear, CancellationToken cancellationToken = default)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails, cancellationToken);

            var items = (
                    await _calendarWorkScheduleItems
                    .WithMany(x => x.WorkScheduleItemExclusions)
                    .ToArrayAsync(x =>
                        x.CalendarWorkScheduleID == workScheduleID
                        && (!dayOfYear.HasValue || x.DayOfYear == dayOfYear.Value), cancellationToken))
                .SelectMany(x => x.WorkScheduleItemExclusions);

            return _mapper.Map<CalendarWorkScheduleItemExclusionDetails[]>(items);
        }

        private async Task<Guid> AddAsync(CalendarWorkScheduleData calendarWorkScheduleDetails, ObjectAction action, CancellationToken cancelationToken)
        {
            await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, action, cancelationToken);

            _logger.LogTrace($"UserID = {_currentUser.UserId} start {action} default {nameof(CalendarWorkSchedule)}");

            var entity = _mapper.Map<CalendarWorkSchedule>(calendarWorkScheduleDetails);
            _calendarWorkScheduleRepository.Insert(entity);
            GenerateDaysForCalendarWorkShedule(entity.ID, entity.Year);
            await _saveChangesCommand.SaveAsync(cancelationToken);

            _logger.LogTrace($"UserID = {_currentUser.UserId} finish {action} default {nameof(CalendarWorkSchedule)}");

            return entity.ID;
        }

        public void GenerateDaysForCalendarWorkShedule(Guid calendarID, int year)
        {
            _logger.LogTrace($"UserID = {_currentUser.UserId} start {ObjectAction.Insert} default {nameof(CalendarWorkScheduleItem)}s to {nameof(CalendarWorkSchedule)} for year {year} ");

            var countDaysInYear = DateTime.IsLeapYear(year) ? 366 : 365;

            for (short dayNumber = 1; dayNumber <= countDaysInYear; dayNumber++)
            {
                var item = new CalendarWorkScheduleItem()
                {
                    TimeSpanInMinutes = 0,
                    CalendarWorkScheduleID = calendarID,
                    DayOfYear = dayNumber,
                    ShiftNumber = null,
                    DayType = CalendarDayType.Weekend,
                    TimeStart = DateTime.MinValue,
                };

                _calendarWorkScheduleItems.Insert(item);
            }

            _logger.LogTrace($"UserID = {_currentUser.UserId} finished {ObjectAction.Insert} default {nameof(CalendarWorkScheduleItem)}s to {nameof(CalendarWorkSchedule)} for year {year} ");
        }
        #endregion
    }
}

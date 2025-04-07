using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceItems;

internal class ServiceItemBLL : IServiceItemBLL, ISelfRegisteredService<IServiceItemBLL>
{
    private readonly IRepository<CallSummary> _callSummaryRepository;
    private readonly IReadonlyRepository<CustomControl> _customControls;
    private readonly ICurrentUser _currentUser;
    private readonly ICatalogFinder<ServiceItem, Guid> _serviceItemFinder;
    private readonly ICatalogFinder<ServiceAttendance, Guid> _serviceAttendanceFinder;
    private readonly IMapper _mapper;

    public ServiceItemBLL(
        IRepository<CallSummary> callSummaryRepository,
        IReadonlyRepository<CustomControl> customControls,
        ICurrentUser currentUser,
        ICatalogFinder<ServiceItem, Guid> serviceItemFinder,
        ICatalogFinder<ServiceAttendance, Guid> serviceAttendanceFinder,
        IMapper mapper)
    {
        _callSummaryRepository = callSummaryRepository;
        _customControls = customControls;
        _currentUser = currentUser;
        _serviceItemFinder = serviceItemFinder;
        _serviceAttendanceFinder = serviceAttendanceFinder;
        _mapper = mapper;
    }

    public async Task<ServiceItemDetailsModel> DetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var item = await _serviceItemFinder.FindAsync(id, cancellationToken);
        var attendance = await _serviceAttendanceFinder.FindAsync(id, cancellationToken);

        var result = item != null
            ? _mapper.Map<ServiceItemDetailsModel>(item)
            : attendance != null ? _mapper.Map<ServiceItemDetailsModel>(attendance) : null;

        result.IsInFavorite = await _customControls.AnyAsync(x => 
            x.UserId == _currentUser.UserId 
            && x.ObjectClass == result.ClassID
            && x.ObjectId == result.ID
            , cancellationToken);

        return result ?? throw new ObjectNotFoundException($"Service Item/Attendance (ID = {id})");
    }

    public async Task<ServiceItemDetailsModel> DetailsByCallSummaryIDAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var callSummary = await _callSummaryRepository.FirstOrDefaultAsync(c => c.ID == id, cancellationToken)
                          ?? throw new ObjectNotFoundException($"CallSummary (ID = {id})");

        var itemOrAttendanceID = callSummary.ServiceItemID ?? callSummary.ServiceAttendanceID;

        return await DetailsAsync(itemOrAttendanceID.Value, cancellationToken);
    }
}

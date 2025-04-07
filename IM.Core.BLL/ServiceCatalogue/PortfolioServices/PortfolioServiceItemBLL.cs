using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices;

internal class PortfolioServiceItemBLL<TDetails, TData> : IPortfolioServiceItemBLL<TDetails, TData>
     where TDetails : PortfolioServcieItemDetails
     where TData : PortfolioServcieItemData
{
    private readonly IMapper _mapper;
    private readonly ISupportLineBLL _supportLineBLL;
    private readonly IServiceTagForServiceFamilyBLL _serviceTagForServiceFamilyBLL;
    private readonly IReadonlyRepository<ServiceItem> _serviceItemsRepository;
    private readonly IReadonlyRepository<ServiceAttendance> _serviceAttendancesRepository;

    public PortfolioServiceItemBLL(IMapper mapper
                                   , ISupportLineBLL supportLineBLL
                                   , IServiceTagForServiceFamilyBLL serviceTagForServiceFamilyBLL
                                   , IReadonlyRepository<ServiceItem> serviceItemsRepository
                                   , IReadonlyRepository<ServiceAttendance> serviceAttendancesRepository)
    {
        _mapper = mapper;
        _supportLineBLL = supportLineBLL;
        _serviceTagForServiceFamilyBLL = serviceTagForServiceFamilyBLL;
        _serviceItemsRepository = serviceItemsRepository;
        _serviceAttendancesRepository = serviceAttendancesRepository;
    }

    public async Task SaveSupportLinAndTagsAsync(Guid id, TData intputModel, CancellationToken cancellationToken)
    {
        var model = _mapper.Map<PortfoliServiceItemSaveModel>(intputModel);
        // у details обязательно должен быть сгенерированный ID, для дальнейшего сохранения линий поддержки и тегов
        model.ID = id;

        model.SupportLineResponsibles.ForEach(c => c.ObjectID = id);
        await _supportLineBLL.SaveAsync(model.SupportLineResponsibles, id, model.ClassID, cancellationToken);
        if (!model.State.HasValue)
            await CopySupportLineToSubItemsAsync(model, cancellationToken);

        await _serviceTagForServiceFamilyBLL.SaveAsync(model.Tags, id, model.ClassID, cancellationToken);
    }

    private async Task CopySupportLineToSubItemsAsync(PortfoliServiceItemSaveModel service, CancellationToken cancellationToken)
    {
        if (service.ClassID != ObjectClass.Service)
            return;

        await CopySuportLineToItemsAsync(service.ID, cancellationToken);
        await CopySuportLineToAttendancesAsync(service.ID, cancellationToken);
    }

    private async Task CopySuportLineToItemsAsync(Guid serviceID, CancellationToken cancellationToken)
    {
        var serviceItems = await _serviceItemsRepository.ToArrayAsync(c => c.ServiceID == serviceID && !c.State.HasValue, cancellationToken);
        foreach (var item in serviceItems)
            await _supportLineBLL.CopySupportLineFromServiceAsync(item.ID, item.ServiceID.Value, ObjectClass.ServiceItem, cancellationToken);
    }


    private async Task CopySuportLineToAttendancesAsync(Guid serviceID, CancellationToken cancellationToken)
    {
        var serviceAttendances = await _serviceAttendancesRepository.ToArrayAsync(c => c.ServiceID == serviceID && !c.State.HasValue, cancellationToken);
        foreach (var item in serviceAttendances)
            await _supportLineBLL.CopySupportLineFromServiceAsync(item.ID, item.ServiceID.Value, ObjectClass.ServiceAttendance, cancellationToken);
    }


    public async Task InitializateSupportLineAndTagsAsync(TDetails details, CancellationToken cancellationToken)
    {
        details.SupportLineResponsibles = await _supportLineBLL.GetResponsibleObjectLineAsync(details.ID, details.ClassID, cancellationToken);
        details.Tags = await _serviceTagForServiceFamilyBLL.GetByIDAndClassIdAsync(details.ID, details.ClassID, cancellationToken);
        details.TagNames = string.Join(", ", details.Tags.Select(c => c.Tag));
    }
}

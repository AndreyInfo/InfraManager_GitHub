using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.AccessManagement;
using InfraManager.BLL.Settings;
using InfraManager.Core.Exceptions;
using InfraManager.DAL.Interface.ServiceDesk.ChangeRequests.RFCGantt;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests.RFCGantt;

internal class RFCGanttBLL : IRFCGanttBLL, ISelfRegisteredService<IRFCGanttBLL>
{
    private readonly IRFCGanttQuery _rfcGanttQuery;
    private readonly ISettingsBLL _settingsBLL;
    private readonly IConvertSettingValue<int> _converterToInt;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;
    private readonly IUserAccessBLL _userAccess;

    public RFCGanttBLL(
        IRFCGanttQuery rfcGanttDataProvider,
        ISettingsBLL settingsBLL,
        IConvertSettingValue<int> converterToInt,
        IMapper mapper,
        ICurrentUser currentUser,
        IUserAccessBLL userAccess)
    {
        _rfcGanttQuery = rfcGanttDataProvider;
        _settingsBLL = settingsBLL;
        _converterToInt = converterToInt;
        _mapper = mapper;
        _currentUser = currentUser;
        _userAccess = userAccess;
    }

    public async Task<IEnumerable<RFCGanttDetails>> GetAsync(CancellationToken cancellationToken = default)
    {
        var sizeProject = await GetViewSizeAsync(cancellationToken);

        var dayAndTime = DateTime.UtcNow;
        var today = dayAndTime.Date;
        var dayForSecondDay = today.AddSeconds(86399);
        var secondDay = dayForSecondDay.AddMonths(sizeProject);
        today = today.AddHours(-168);

        var reportData = await _rfcGanttQuery.ExecuteAsync(today, secondDay, cancellationToken);
        if (reportData == null)
            throw new ObjectDeletedException("RFCGennt Get");

        return reportData.Select(x => _mapper.Map<RFCGanttDetails>(x)).ToList();
    }

    public async Task<int> GetViewSizeAsync(CancellationToken cancellationToken = default)
    {
        if (!await _userAccess.HasRolesAsync(_currentUser.UserId, cancellationToken))
            throw new AccessDeniedException($"User Id={_currentUser.UserId} Don't have rights");

        var settingValue = await _settingsBLL.GetValueAsync(SystemSettings.SizeProject, cancellationToken);

        return _converterToInt.Convert(settingValue);
    }
}
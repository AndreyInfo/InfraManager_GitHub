using AutoMapper;
using InfraManager.BLL.Asset;
using InfraManager.BLL.Localization;
using InfraManager.BLL.Report;
using InfraManager.BLL.ReportsForCommand;
using InfraManager.BLL.Settings;
using InfraManager.Core.Extensions;
using InfraManager.ResourcesArea;
using InfraManager.WebApi.Contracts.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.ReportsForCommand;

public class ReportsForCommandBLL : IReportsForCommandBLL
    , ISelfRegisteredService<IReportsForCommandBLL>
{
    private readonly ILocalizeText _localizeText;
    private readonly IMapper _mapper;
    private readonly ISettingsBLL _settingsBLL;
    private readonly IConvertSettingValue<List<ReportForCommandData>> _convert;
    private readonly IReportBLL _reportBLL;

    public ReportsForCommandBLL(
          ILocalizeText localizeText
        , IMapper mapper
        , ISettingsBLL settingsBLL
        , IConvertSettingValue<List<ReportForCommandData>> convert
        , IReportBLL reportBLL
        )
    {
        _localizeText = localizeText;
        _mapper = mapper;
        _settingsBLL = settingsBLL;
        _convert = convert;
        _reportBLL = reportBLL;
    }

    public async Task<ReportForCommandDetails> GetAsync(byte id, CancellationToken cancellationToken)
    {
        var reportCommands = await GetReportCommandsAsync(cancellationToken);

        ReportForCommandData command = null;

        command = reportCommands.FirstOrDefault(x => x.OperationType == (OperationType)id);

        if (command is not null)
        {
            var details = _mapper.Map<ReportForCommandDetails>(command);
            var report = await _reportBLL.GetReportAsync(command.ReportID, cancellationToken);
            details.ReportName = report.Name;
            details.StringFolder = report.FolderName;
            return details;
        }

        return new ReportForCommandDetails()
        {
            OperationType = (OperationType)id
        };
      }

    public async Task<ReportForCommandDetails[]> GetListAsync(CancellationToken cancellationToken)
    {
        var commands = await GetReportCommandsAsync(cancellationToken);

        var details = _mapper.Map<ReportForCommandDetails[]>(commands);

        foreach (var item in details)
        {
            var report = await _reportBLL.GetReportAsync((Guid)item.ReportID, cancellationToken);
            item.ReportName = report.Name;
            item.StringFolder = report.FolderName;
        }

        return details;
    }

    public async Task<ReportForCommandDetails> AddAsync(ReportForCommandData data, CancellationToken cancellationToken)
    {
        CheckCommand((byte)data.OperationType);

        var report = await _reportBLL.GetReportAsync(data.ReportID, cancellationToken);

        var commands = await GetReportCommandsAsync(cancellationToken);

        if (commands is not null)
            CheckData(data, commands);

        if (commands is null)
            commands = new List<ReportForCommandData>();

        commands.Add(data);
        
        await SaveReportCommandsAsync(commands, cancellationToken);

        var details = _mapper.Map<ReportForCommandDetails>(data);
        
        details.ReportName = report.Name;
        details.StringFolder = report.FolderName;

        return details;
    }

    public async Task<ReportForCommandDetails> UpdateAsync(byte id, ReportForCommandData data, CancellationToken cancellationToken)
    {
        CheckCommand((byte)data.OperationType);

        var report = await _reportBLL.GetReportAsync(data.ReportID, cancellationToken);

        var commands = await GetReportCommandsAsync(cancellationToken);

        var command = commands.FirstOrDefault(x => (byte)x.OperationType == id);

        command.ReportID = data.ReportID;

        await SaveReportCommandsAsync(commands, cancellationToken);

        var details = _mapper.Map<ReportForCommandDetails>(command);

        details.ReportName = report.Name;
        details.StringFolder = report.FolderName;

        return details;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var reportCommands = await GetReportCommandsAsync(cancellationToken);
        reportCommands.Remove(reportCommands.FirstOrDefault(x => x.OperationType == (OperationType)id));
        await SaveReportCommandsAsync(reportCommands, cancellationToken);
    }

    private void CheckData(ReportForCommandData data, List<ReportForCommandData> list)
    {
        foreach (var item in list)
        {
            if (item.OperationType == data.OperationType)
            {
                throw new InvalidObjectException(string.Format(_localizeText.Localize(nameof(Resources.ReportsForCommand_ReportExists))));
            }
        }
    }

    private void CheckCommand(byte id)
    {
        if (!Enum.IsDefined(typeof(OperationType), id))
        {
            throw new InvalidObjectException(string.Format(_localizeText.Localize(nameof(Resources.ReportsForCommand_CommandNotFound))));
        }
    }

    private async Task<List<ReportForCommandData>> GetReportCommandsAsync(CancellationToken cancellationToken)
    {
        var setting = await _settingsBLL.GetAsync(SystemSettings.AssetOperationReports, cancellationToken);
        return _convert.Convert(setting.Value as byte[]);
    }

    private async Task SaveReportCommandsAsync(List<ReportForCommandData>  reportCommands, CancellationToken cancellationToken)
    {
        var settingData = new SettingData { Value = _convert.ConvertBack(reportCommands) };
        await _settingsBLL.SetAsync(SystemSettings.AssetOperationReports, settingData, cancellationToken);
    }
}
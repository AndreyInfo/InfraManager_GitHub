using AutoMapper;
using IM.Core.Import.BLL.Interface.Exceptions;
using IM.Core.Import.BLL.Interface.Import.CSV;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.Models.Import;
using IM.Core.Import.BLL.Interface.Import.Models.Settings;
using IM.Core.Import.BLL.Interface.Import.Models.UploadData;
using InfraManager;
using InfraManager.BLL;
using ObjectNotFoundException = IM.Core.Import.BLL.Interface.Exceptions.ObjectNotFoundException;

namespace IM.Core.Import.BLL.Import.Importer;

public class Importer<TData>:IImporter
{
    private const int PacketSize = 5000;
   
    
    private readonly ISaver<TData> _saver;
    private readonly IValidator<TData> _validator;
    private readonly IScriptDataParser<ConcordanceObjectType> _parser;
    private readonly ICsvStringReader _reader;
    private readonly ICsvRepository _csvRepository;
    private readonly IScriptDataBLL _scriptDataBLL;
    private readonly IDataFactory<TData> _factory;
    private readonly IMapper _mapper;
    public Importer(
        ISaver<TData> saver, 
        IValidator<TData> validator,
         IScriptDataParser<ConcordanceObjectType> parser,
         ICsvStringReader reader, 
         ICsvRepository csvRepository, 
         IScriptDataBLL scriptDataBLL, 
         IDataFactory<TData> factory, 
         IMapper mapper
        )
    {
        _saver = saver;
        _validator = validator;
         _parser = parser;
         _reader = reader;
         _csvRepository = csvRepository;
         _scriptDataBLL = scriptDataBLL;
         _factory = factory;
         _mapper = mapper;
    }

    private async Task RunAsyncInternal(Guid id, Func<TData, CancellationToken, Task> asyncAction,
         CancellationToken token = default)
    {
        //todo: вынести сервис
        var packetSize = 0;

        var options = await _scriptDataBLL.GetCsvOptions(id, token);
        if (options == null)
            throw new ObjectNotFoundException($"Не найдена настройка импорта с id = {id}");
        var delimeter = options.Delimeter;
        using var fileReader = await _csvRepository.GetCsvStreamReader(id, token);
        var header = System.Array.Empty<string>();
        if (!fileReader.EndOfStream)
            header = _reader.GetData(fileReader, delimeter).ToArray();
        var scripts = await _scriptDataBLL.GetScriptsAsync(id, token);
        var dataHeader = scripts.Select(x => x.FieldName).ToArray();
        var scriptsData = _parser.GetParserData(scripts);

        while (!fileReader.EndOfStream)
        {
            var csvString = _reader.GetData(fileReader, delimeter).ToArray();
            var dataRow = _parser.ParseToDictionary(scriptsData, header, csvString);

            var model = _factory.BuildData(dataHeader, dataRow.Values.Cast<string>().ToArray());

            await asyncAction(model, token);
        }
    }

    
    
    public async Task RunAsync(Guid id, bool withDelete = false, CancellationToken token = default)
    {
        //todo: вынести сервис
        var packetSize = 0;

        async Task LoadData(TData model, CancellationToken token)
        {
            if (!(await _validator.ValidateAsync(id, model, token)))
               return;

            await _saver.AddToBatchDataAsync(model, token);

            packetSize++;
            if (packetSize >= PacketSize)
            {
                packetSize = 0;
                await _saver.SaveBatchAsync(token);
            }
        }

        await RunAsyncInternal(id, LoadData, token);
        
        await _saver.SaveBatchAsync(token);
        
        if (withDelete)
            await _saver.DeleteAsync(token);
    }
    
    public async Task<IEnumerable<CommonData>> GetNotLoadedDataAsync(Guid id, CancellationToken token)
    {
        //todo: вынести сервис
        var errorData = new List<CommonData>();

        async Task GetErrorData(TData data, CancellationToken token)
        {
            if (await _validator.ValidateAsync(id, data, token))
                return;
            var commonData = _mapper.Map<CommonData>(data);
            errorData.Add(commonData);
        }

        await RunAsyncInternal(id, GetErrorData, token);

        return errorData;
    }
}
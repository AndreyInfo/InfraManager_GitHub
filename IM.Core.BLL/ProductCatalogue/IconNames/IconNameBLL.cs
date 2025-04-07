using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.IconNames;

public class IconNameBLL:IIconNameBLL,ISelfRegisteredService<IIconNameBLL>
{
    private readonly IReadonlyRepository<Icon> _iconNames;
    private readonly IMapper _mapper;
    public IconNameBLL(IReadonlyRepository<Icon> iconNames, IMapper mapper)
    {
        _iconNames = iconNames;
        _mapper = mapper;
    }

    public async Task<IconNameDetails> GetIconAsync(Guid iconID, CancellationToken token = default)
    {

        var iconNameData = await _iconNames.FirstOrDefaultAsync(x=>x.ID == iconID,token);

        return _mapper.Map<IconNameDetails>(iconNameData);
    }

    public async Task<IconNameDetails[]> GetIconsAsync(CancellationToken token)
    {
        var icons = await _iconNames.ToArrayAsync(token);
        return _mapper.Map<IconNameDetails[]>(icons);
    }
}
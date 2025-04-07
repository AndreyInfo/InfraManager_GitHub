using AutoMapper;
using InfraManager.DAL.ProductCatalogue;

namespace InfraManager.BLL.ProductCatalogue.IconNames;

public class IconNameProfile:Profile
{
    public IconNameProfile()
    {
        CreateMap<Icon, IconNameDetails>().ReverseMap();
    }
}
using AutoMapper;

namespace InfraManager.BLL.Software.SoftwareModelProcessNames;

public class SoftwareModelProcessNameProfile : Profile
{
    public SoftwareModelProcessNameProfile()
    {
        CreateMap<string, SoftwareModelProcessNameDetails>()
            .ConvertUsing((text, processName) => new SoftwareModelProcessNameDetails { ProcessName = text });
    }
}

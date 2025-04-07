using AutoMapper;
using InfraManager.DAL;

namespace InfraManager.BLL.AutoMapper
{
    public class DescriptionProfile : Profile
    {
        public DescriptionProfile()
        {
            CreateMap<Description, string>().ConvertUsing(description => description.Formatted);
            CreateMap<string, Description>()
                .ConvertUsing(
                    (text, description) =>
                        text == null || text == description.Formatted
                            ? description
                            : new Description(text.RemoveHtmlTags(), text, description));
        }
    }
}

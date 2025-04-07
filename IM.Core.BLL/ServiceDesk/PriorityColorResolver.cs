using AutoMapper;
using InfraManager.Core.Helpers;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk
{
    public class PriorityColorResolver
        : IValueResolver<ISDEntityWithPriorityColorInt, object, string>,
            IValueResolver<IEntityWithColorInt, object, string>
    {
        public string Resolve(ISDEntityWithPriorityColorInt source, object destination, string destMember, ResolutionContext context)
        {
            if(source.PriorityColor == 0)
            {
                return null;
            }

            return $"#{source.PriorityColor.ColorFromArgb().ToHtmlColor()}";
        }

        public string Resolve(IEntityWithColorInt source, object destination, string destMember, ResolutionContext context)
        {
            return $"#{source.Color.ColorFromArgb().ToHtmlColor()}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.Settings;

namespace InfraManager.BLL.Notification
{
    public class DefaultNotificationTreeItemProfile : Profile
    {
        public DefaultNotificationTreeItemProfile()
        {
            CreateMap<(NotificationTreeLevel level, int parentID, bool HasChild, bool partSelected,
                    int id, string name), DefaultNotificationTreeItem>()
                .ForMember(x => x.ID, z => z.MapFrom(x => x.id))
                .ForMember(x => x.Name, z => z.MapFrom(x => x.name))
                .ForMember(x => x.HasChild, z => z.MapFrom(x => x.HasChild))
                .ForMember(x => x.PartSelected, z => z.MapFrom(x => x.partSelected))
                .ForMember(x => x.Level, z => z.MapFrom(x => x.level))
                .ForMember(x => x.PartSelected, z => z.MapFrom(x => x.partSelected))
                .ForMember(x => x.ClassId, z => z.MapFrom(x => x.parentID));

        }
    }
}

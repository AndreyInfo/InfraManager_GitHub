using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.CalendarService
{
    public class CalendarProfile : Profile
    {
        public CalendarProfile()
        {
            CreateMap<TimeZoneInfo.AdjustmentRule, DAL.ServiceDesk.TimeZoneAdjustmentRule>();
        }
    }
}

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Quality
{
    public class QualityControlProfile : Profile
    {
        public QualityControlProfile()
        {
            CreateMap<QualityControlData, DAL.ServiceDesk.Quality.QualityControl>();

            CreateMap<DAL.ServiceDesk.Quality.QualityControl, QualityControlDetails>();
        }
    }
}

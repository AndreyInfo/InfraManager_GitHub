using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk.Quality
{
    public  enum QualityControlType : byte
    {
        //  [FriendlyName("Создание заявки")]
        CreateCall = 0,
        //  [FriendlyName("Автоматический перерасчет")]
        SLA_AutoRefresh = 1,
        //  [FriendlyName("Ручной перерасчет")]
        SLA_CostumerRefresh = 2,
        //  [FriendlyName("Таймер остановлен")]
        TimerSuspend = 3,
        //  [FriendlyName("Таймер запущен")]
        TimerResume = 4,
        //  [FriendlyName("Завершена")]
        WorkflowTerminated = 5
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk
{
    public class NoteListItemModel
    {
        public Guid ID { get; init; }
        public string DateForJs { get; init; }
        public DateTime UtcDate { get; init; }
        public Guid UserID { get; init; }
        public string Message { get; init; }
        public string UserName { get; init; }
        public bool IsRead { get; init; }
        public bool IsNote { get; init; }

    }
}

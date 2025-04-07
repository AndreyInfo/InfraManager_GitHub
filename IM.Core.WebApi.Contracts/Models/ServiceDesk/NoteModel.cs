using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk
{
    public class NoteModel
    {
        public string Message { get; init; }
        public bool? IsRead { get; init; }
        public bool? IsNote { get; init; }

    }
}

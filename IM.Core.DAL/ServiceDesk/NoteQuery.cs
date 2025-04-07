using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InfraManager.DAL.ServiceDesk
{
    internal class NoteQuery<TNote> : INoteQuery<TNote>
    {
        private readonly DbSet<Note<TNote>> _set;
        private readonly IMapper _mapper;

        public NoteQuery(DbSet<Note<TNote>> set, IMapper mapper)
        {
            _set = set;
            _mapper = mapper;
        }

        public IQueryable<NoteData> Query()
        {
            return _mapper.ProjectTo<NoteData>(_set);
        }
    }
}

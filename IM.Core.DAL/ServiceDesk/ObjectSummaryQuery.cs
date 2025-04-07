using AutoMapper;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk
{
    internal class ObjectSummaryQuery<TEntity> : IObjectSummaryInfoQuery<TEntity>
        where TEntity: class
    {
        private readonly DbContext _db;
        private readonly INoteQuery<TEntity> _notesQuery;
        private readonly IMapper _mapper;

        public ObjectSummaryQuery(CrossPlatformDbContext db, INoteQuery<TEntity> notesQuery, IMapper mapper)
        {
            _db = db;
            _notesQuery = notesQuery;
            _mapper = mapper;
        }

        public async Task<ObjectSummaryInfo> ExecuteAsync(Guid objectID, Guid userID, CancellationToken cancellationToken = default)
        {
            var objectClassID = typeof(TEntity).GetObjectClassOrRaiseError();
            var innerQuery = _mapper.ProjectTo<ExecutableInfo>(_db.Set<TEntity>());
            var customControls = _db.Set<CustomControl>()
                .Where(x => x.ObjectClass == objectClassID && x.UserId == userID);
            var groupUsers = _db.Set<GroupUser>().Where(x => x.UserID == userID);
            var notesQuery = _notesQuery.Query()
                .Where(note => note.ParentObjectID == objectID)
                .GroupBy(note => note.Type)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToArray();
            var messages = notesQuery.FirstOrDefault(x => x.Type == SDNoteType.Message)?.Count ?? 0;
            var notes = notesQuery.FirstOrDefault(x => x.Type == SDNoteType.Note)?.Count ?? 0;

            var query = 
                from entity in innerQuery.Where(x => x.IMObjID == objectID)
                select new ObjectSummaryInfo
                {
                    MessageCount = messages,
                    NoteCount = notes,
                    InControl = customControls.Any(x => x.ObjectId == entity.IMObjID),
                    CanBePicked =  groupUsers.Any(x => x.GroupID == entity.GroupID)
                        && entity.ExecutorID != User.NullUserGloablIdentifier
                };

            return await query.SingleOrDefaultAsync(cancellationToken);
        }
    }
}

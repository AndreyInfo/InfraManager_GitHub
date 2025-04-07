using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InfraManager.DAL.ServiceDesk
{
    internal class IssueTypesQuery<TEntity, TType> : IQueryIssueTypes
        where TEntity : class
        where TType : class
    {
        private readonly DbSet<TEntity> _entities;
        private readonly DbSet<TType> _types;
        private readonly IMapper _mapper;

        public IssueTypesQuery(DbSet<TEntity> entities, DbSet<TType> types, IMapper mapper)
        {
            _entities = entities;
            _types = types;
            _mapper = mapper;
        }

        public IQueryable<IssueType> Query()
        {
            var typesQuery = _mapper.ProjectTo<IssueType>(_types);
            var entityTypeReferences = _mapper.ProjectTo<IssueTypeReference>(_entities);

            return typesQuery.Where(t => entityTypeReferences.Any(x => x.TypeID == t.ID));
        }
    }
}

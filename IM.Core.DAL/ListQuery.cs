using AutoMapper.QueryableExtensions;
using InfraManager.DAL.AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL
{
    internal class ListQuery<TEntity, TQueryResultItem> : IListQuery<TEntity, TQueryResultItem> 
        where TEntity : class
    {
        private readonly DbSet<TEntity> _dbset;
        private readonly ICreateConfigurationProvider<TEntity, TQueryResultItem, Guid> _mappingCreator;

        public ListQuery(DbSet<TEntity> dbset, ICreateConfigurationProvider<TEntity, TQueryResultItem, Guid> mappingCreator)
        {
            _dbset = dbset;
            _mappingCreator = mappingCreator;
        }

        public IQueryable<TQueryResultItem> Query(Guid userID, IEnumerable<Expression<Func<TEntity, bool>>> predicates)
        {
            return _dbset.Where(predicates).ProjectTo<TQueryResultItem>(_mappingCreator.Create(userID));
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL
{
    internal static class DbSetExtensions
    {
        public static IQueryable<T> Where<T>(this DbSet<T> dbSet, IEnumerable<Expression<Func<T, bool>>> where)
            where T : class
        {
            var result = dbSet.AsQueryable();
            foreach(var whereStatement in where)
            {
                result = result.Where(whereStatement);
            }
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.DAL.Settings
{
    /// <inheritdoc cref="ICustomUserBuilderQueryListSoftwareInstallation" />
    internal class CustomUserListQuery : 
        ICustomFilterListQuery,
        ISelfRegisteredService<ICustomFilterListQuery>        
    {
        /// <summary>
        /// DataContext для работы со инсталляциями
        /// </summary>
        private readonly CrossPlatformDbContext _db;

        public CustomUserListQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }
 
        public IQueryable<T> Query <T> (IEnumerable<FilterElementData> filterElements) where T : class
        {
            IQueryable<T> query = _db.Set<T>();

            foreach (var filterElement in filterElements)
            {                             
                query = query.Where(
                    filterElement.PropertyName, 
                    ParseComparisonType(filterElement.SearchOperation), 
                    filterElement.SearchValue);
            }

            return query;
        }

        private ComparisonType ParseComparisonType(byte? comparisonTypeInt)
        {
            switch (comparisonTypeInt)
            {
                case 0: return ComparisonType.Contains;

                case 1: return ComparisonType.Equal;

                default:
                    throw new ArgumentOutOfRangeException($"Не поддерживаемый тип {comparisonTypeInt}");
            }        
        }
    }
}

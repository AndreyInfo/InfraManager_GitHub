using InfraManager.DAL.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.DAL.Software.Installation
{
    /// <inheritdoc cref="ICustomUserBuilderQueryListSoftwareInstallation" />
    internal class CustomUserSoftwareInstallationListQuery 
        : ICustomFilterSoftwareInstallationListQuery,
        ISelfRegisteredService<ICustomFilterSoftwareInstallationListQuery>        
    {
        /// <summary>
        /// DataContext для работы со инсталляциями
        /// </summary>
        private readonly CrossPlatformDbContext _db;

        public CustomUserSoftwareInstallationListQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        // TODO: Эта Query должна быть generic
        /// <inheritdoc />        
        public IQueryable<ViewSoftwareInstallation> Query(IEnumerable<FilterElementData> filterElements)
        {
            IQueryable<ViewSoftwareInstallation> query = _db.Set<ViewSoftwareInstallation>();

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

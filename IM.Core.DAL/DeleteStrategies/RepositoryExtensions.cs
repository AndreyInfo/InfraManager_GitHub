using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InfraManager.DAL.DeleteStrategies
{
    internal static class RepositoryExtensions
    {
        public static void RemoveObjectIconIfExists<T>(
            this DbSet<ObjectIcon> objectIcons,
            T entity) where T : IGloballyIdentifiedEntity
        {
            var objectIcon = objectIcons.FirstOrDefault(ObjectIcon.WhereObjectEquals(entity));

            if (objectIcon != null)
            {
                objectIcons.Remove(objectIcon);
            }
        }
    }
}

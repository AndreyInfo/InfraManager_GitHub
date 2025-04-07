using Inframanager.BLL;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.BLL.ServiceCatalogue
{
    public class ServiceCategoryListFilter : ClientPageFilter<ServiceCategory>
    {
        public ServiceCategoryListFilter()
        {
            OrderByProperty = nameof(ServiceCategory.Name); // Сортировка по умолчанию
        }

        // TODO: Добавить свойства по мере реализации списков категорий сервисов
    }
}

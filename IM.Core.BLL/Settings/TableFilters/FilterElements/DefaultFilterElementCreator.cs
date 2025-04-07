using InfraManager.DAL.Settings;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements
{
    public class DefaultFilterElementCreator : ICreateFilterElement
    {
        public FilterElementBase CreateFilterElement(FilterElementData elementData)
        {
            return new DefaultFilterElement(elementData);
        }

        public FilterElementBase CreateFilterElement(FilterElementDetails model)
        {
            return new DefaultFilterElement(model);
        }
    }
}

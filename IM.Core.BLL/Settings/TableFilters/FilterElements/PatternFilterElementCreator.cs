using InfraManager.DAL.Settings;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements
{
    internal class PatternFilterElementCreator : ICreateFilterElement
    {
        public FilterElementBase CreateFilterElement(FilterElementData elementData)
        {
            return new PatternFilterElement(elementData);
        }

        public FilterElementBase CreateFilterElement(FilterElementDetails model)
        {
            return new PatternFilterElement(model);
        }
    }
}

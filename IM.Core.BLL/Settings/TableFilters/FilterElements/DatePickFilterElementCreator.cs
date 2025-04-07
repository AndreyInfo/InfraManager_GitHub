using InfraManager.DAL.Settings;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements
{
    internal class DatePickFilterElementCreator : ICreateFilterElement
    {
        public FilterElementBase CreateFilterElement(FilterElementData elementData)
        {
            return new DatePickFilterElement(elementData);
        }

        public FilterElementBase CreateFilterElement(FilterElementDetails model)
        {
            return new DatePickFilterElement(model);
        }
    }
}

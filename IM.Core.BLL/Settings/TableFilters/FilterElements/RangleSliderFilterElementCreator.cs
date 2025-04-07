using InfraManager.DAL.Settings;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements
{
    internal class RangleSliderFilterElementCreator : ICreateFilterElement
    {
        private readonly IParser _parser;

        public RangleSliderFilterElementCreator(IParser parser)
        {
            _parser = parser;
        }

        public FilterElementBase CreateFilterElement(FilterElementData elementData)
        {
            return new RangeSliderFilterElement(elementData, _parser);
        }

        public FilterElementBase CreateFilterElement(FilterElementDetails model)
        {
            return new RangeSliderFilterElement(model, _parser);
        }
    }
}

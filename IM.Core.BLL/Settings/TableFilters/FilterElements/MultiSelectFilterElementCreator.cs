using InfraManager.DAL.Settings;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements
{
    public class MultiSelectFilterElementCreator : ICreateFilterElement
    {
        private readonly IArrayExpressionBuilder _expressionBuilder;

        public MultiSelectFilterElementCreator(IArrayExpressionBuilder expressionBuilder)
        {
            _expressionBuilder = expressionBuilder;
        }

        public FilterElementBase CreateFilterElement(FilterElementData elementData)
        {
            return new MultiSelectFilterElement(elementData, _expressionBuilder);
        }

        public FilterElementBase CreateFilterElement(FilterElementDetails model)
        {
            return new MultiSelectFilterElement(model, _expressionBuilder);
        }
    }
}

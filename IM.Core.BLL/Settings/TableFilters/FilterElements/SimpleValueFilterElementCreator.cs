using InfraManager.DAL.Settings;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements
{
    internal class SimpleValueFilterElementCreator : ICreateFilterElement
    {
        private readonly IServiceMapper<string, IBuildTreeSettings> _treeSettingsBuilders;
        private readonly IArrayExpressionBuilder _expressionBuilder;

        public SimpleValueFilterElementCreator(
            IServiceMapper<string, IBuildTreeSettings> treeSettingsBuilders, 
            IArrayExpressionBuilder expressionBuilder)
        {
            _treeSettingsBuilders = treeSettingsBuilders;
            _expressionBuilder = expressionBuilder;
        }

        public FilterElementBase CreateFilterElement(FilterElementData elementData)
        {
            var treeSettings = _treeSettingsBuilders
                .Map(elementData.ClassSearcher)
                .Build(elementData.SearcherParams);

            return new SimpleValueFilterElement(elementData, treeSettings, _expressionBuilder);
        }

        public FilterElementBase CreateFilterElement(FilterElementDetails model)
        {
            return new SimpleValueFilterElement(model, _expressionBuilder);
        }
    }
}

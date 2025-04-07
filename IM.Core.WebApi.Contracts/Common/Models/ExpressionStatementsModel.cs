namespace InfraManager.CrossPlatform.WebApi.Contracts.Common.Models
{
    public class ExpressionStatementsModel
    {
        public StatementDefinitionModel[] Variables { get; set; }
        public StatementDefinitionModel[] Functions { get; set; }
        public StatementDefinitionModel[] Operators { get; set; }
    }

    public class StatementDefinitionModel
    {
        public StatementDefinitionModel()
        {
        }

        public StatementDefinitionModel(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}

namespace InfraManager.BLL.ServiceDesk
{
    public class PriorityModel
    {
        public string Name { get; init; }

        public string Color { get; init; }

        public int Sequence { get; init; }

        public bool Default { get; init; }

        public string RowVersion { get; init; }
    }
}

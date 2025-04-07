namespace InfraManager.DAL.ServiceDesk
{
    public class ObjectSummaryInfo
    {
        public int NoteCount { get; init; }
        public int MessageCount { get; init; }
        public bool InControl { get; init; }
        public bool CanBePicked { get; init; }
    }
}

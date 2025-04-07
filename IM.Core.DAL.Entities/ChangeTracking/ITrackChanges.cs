namespace InfraManager.DAL.ChangeTracking
{
    public interface ITrackChanges
    {
        void SetUnmodified();
    }

    public interface ITrackChangesOf<T> : ITrackChanges
    {
        T Original { get; }        
    }
}

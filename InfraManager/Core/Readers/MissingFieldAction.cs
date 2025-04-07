namespace InfraManager.Core.Readers
{
    public enum MissingFieldAction : byte
    {
        ThrowException = 0,
        ReplaceByEmpty = 1,
        ReplaceByNull = 2
    }
}

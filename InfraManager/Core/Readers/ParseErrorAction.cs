namespace InfraManager.Core.Readers
{
    public enum ParseErrorAction : byte
    {
        ThrowException = 0,
        RaiseEvent = 1,
        AdvanceToNextLine = 2
    }
}

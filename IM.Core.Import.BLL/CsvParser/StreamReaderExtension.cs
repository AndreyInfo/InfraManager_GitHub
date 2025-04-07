namespace IM.Core.Import.BLL.Import.Csv;

public static class StreamReaderExtension
{
    public static char ReadCharOrThrow(this StreamReader reader)
    {
        var buffer = new char[1];
        var count = reader.ReadBlock(buffer);
        if (count == 0)
            throw new InvalidDataException();
        return buffer[0];
    }
}
namespace InfraManager.BLL
{
    public interface IParser
    {
        T Parse<T>(string s);
    }
}

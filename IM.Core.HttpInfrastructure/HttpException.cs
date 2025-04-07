namespace IM.Core.HttpInfrastructure
{
    public class HttpException : Exception
    {
        public HttpException(int httpStatusCode)
        {
            HttpStatusCode = httpStatusCode;
        }

        public HttpException(System.Net.HttpStatusCode httpStatusCode) : this((int)httpStatusCode)
        {
        }

        public int HttpStatusCode { get; }
    }
}

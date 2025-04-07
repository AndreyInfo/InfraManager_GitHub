using System;

namespace InfraManager
{
    public class WebApiException : Exception
    {
        public WebApiException(string message) : base(message) { }
    }
}

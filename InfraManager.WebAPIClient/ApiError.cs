using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace InfraManager.WebAPIClient
{
    public class ApiError
    {
        public ApiError(HttpStatusCode httpStatusCode)
        {
            StatusCode = httpStatusCode;
        }
        public string Path { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; private set; }
        public int Status { get => (int)StatusCode; }
    }
}

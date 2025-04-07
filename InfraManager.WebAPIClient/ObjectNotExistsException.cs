using System;
using System.Collections.Generic;
using System.Text;

namespace InfraManager.WebAPIClient
{
    public class ObjectNotExistsException : Exception
    {
        public ObjectNotExistsException(string message) : base(message) { }
    }
}

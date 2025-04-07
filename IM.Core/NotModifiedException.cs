using System;

namespace Inframanager;

public class NotModifiedException : Exception
{
    public NotModifiedException() { }
    
    public NotModifiedException(string message) : base(message) { }
}
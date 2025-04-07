using System;

namespace InfraManager;

public class RestrictionException : Exception
{
    public RestrictionException(string text) : base(text)
    {
        
    }
}
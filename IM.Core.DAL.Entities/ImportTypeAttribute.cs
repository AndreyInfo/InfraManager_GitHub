using System;

namespace InfraManager.DAL;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class ImportTypeAttribute:Attribute
{
    public string Name { get; init; }
}
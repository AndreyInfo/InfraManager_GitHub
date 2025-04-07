using System;

namespace Inframanager
{
    public class ForeignKeyConstraintViolationException : Exception
    {
        public ForeignKeyConstraintViolationException(string keys, Type entity, Exception inner)
            : base($"{entity.Name}.{keys} foreign key constraint violation", inner)
        {
            Keys = keys;
            Entity = entity;
        }

        public string Keys { get; }
        public Type Entity { get; }
    }
}

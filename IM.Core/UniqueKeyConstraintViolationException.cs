using System;

namespace Inframanager
{
    public class UniqueKeyConstraintViolationException : Exception
    {
        public UniqueKeyConstraintViolationException(string keys, Type entity, Exception inner)
            : base($"{entity.Name}.{keys} unique constraint violation", inner)
        {
            Keys = keys;
            Entity = entity;
        }
        public UniqueKeyConstraintViolationException(string keys)
            :base(keys)
        {
            Keys = keys;
        }

        public string Keys { get; }
        public Type Entity { get; }
    }
}

using Inframanager;
using InfraManager.Linq;
using System;
using System.Linq.Expressions;

namespace Inframanager
{
    public class Negation<T> : Specification<T>
    {
        private readonly Specification<T> _spec;

        public Negation(Specification<T> spec) 
            : base(((Expression<Func<T, bool>>)spec).Not())
        {
            _spec = spec;
        }

        protected override bool Additive => false;
        protected override Func<T, bool> GetFunc()
        {
            return x => !_spec.IsSatisfiedBy(x);
        }
    }
}

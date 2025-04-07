using Inframanager;
using InfraManager.Linq;
using System;
using System.Linq.Expressions;

namespace Inframanager
{
    public class Disjunction<T> : Specification<T>
    {
        private readonly Specification<T> _left;
        private readonly Specification<T> _right;

        public Disjunction(Specification<T> left, Specification<T> right) 
            : base(((Expression<Func<T, bool>>)left).Or(right))
        {
            _left = left;
            _right = right;
        }

        protected override bool Additive => false;
        protected override Func<T, bool> GetFunc()
        {
            return x => _left.IsSatisfiedBy(x) || _right.IsSatisfiedBy(x);
        }
    }
}

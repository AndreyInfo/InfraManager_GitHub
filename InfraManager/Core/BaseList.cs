using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.Core
{
    public enum SortingOrder
    {
        None = 0,
        Asc = 1,
        Desc = 2
    }

    [Serializable]
    public class BaseList<T> : List<T>
    {
        #region classes
        public delegate void ListChangedDelegate(ListChangedType type, T[] collection);
        #endregion

        #region Fields
        [NonSerialized]
        private ListChangedDelegate _listChanged;
        [NonSerialized]
        private ListChangedDelegate _beforeListChanged;
        #endregion

        #region constructors
        public BaseList()
            : base()
        { }

        public BaseList(int capacity)
            : base(capacity)
        { }

        public BaseList(IEnumerable<T> source) :
            base(source)
        { }
        #endregion

        #region Events
        public event ListChangedDelegate ListChanged
        {
            add { _listChanged += value; }
            remove { _listChanged -= value; }
        }
        public event ListChangedDelegate BeforeListChanged
        {
            add { _beforeListChanged += value; }
            remove { _beforeListChanged -= value; }
        }

        private void OnListChanged(ListChangedType type, T[] collection)
        {
            if (_listChanged != null)
                _listChanged(type, collection);
        }

        private void OnBeforeListChanged(ListChangedType type, T[] collection)
        {
            if (_beforeListChanged != null)
                _beforeListChanged(type, collection);
        }
        #endregion

        #region virtual method SortByMember
        public virtual BaseList<T> SortByMember(string memberName, SortingOrder sortOrder)
        {
            if (sortOrder == SortingOrder.None)
                return this;
            //
            ParameterExpression param = Expression.Parameter(typeof(T), string.Empty);
            MemberExpression propertyOrField = Expression.PropertyOrField(param, memberName);
            UnaryExpression convertToObject = Expression.Convert(propertyOrField, typeof(object));
            var selector = Expression.Lambda<Func<T, object>>(convertToObject, param);
            //
            IQueryable<T> queryableList = this.AsQueryable<T>();
            IOrderedQueryable<T> orderedList;
            if (sortOrder == SortingOrder.Asc)
                orderedList = queryableList.OrderBy<T, object>(selector);
            else
                orderedList = queryableList.OrderByDescending<T, object>(selector);
            //
            return new BaseList<T>(orderedList);
        }
        #endregion
        
        #region virtual method Clone
        public virtual BaseList<T> Clone()
        {
            var result = new BaseList<T>();
            result.AddRange(this);
            //
            return result;
        }
        #endregion

        #region virtual method Filter
        public virtual BaseList<T> Filter(Filter<T> filter)
        {
            var result = new BaseList<T>();
            //
            foreach (T obj in this)
                if (filter.Eval(obj))
                    result.Add(obj);
            //
            return result;
        }
        #endregion


        #region new List methods with events
        public new void Add(T item)
        {
            OnBeforeListChanged(ListChangedType.ItemAdded, new T[] { item });
            //
            base.Add(item);
            //
            OnListChanged(ListChangedType.ItemAdded, new T[] { item });
        }

        public new void AddRange(IEnumerable<T> collection)
        {
            var items = collection.ToArray();
            //
            OnBeforeListChanged(ListChangedType.ItemAdded, items);
            //
            base.AddRange(items);
            //
            OnListChanged(ListChangedType.ItemAdded, items);
        }

        public new void Clear()
        {
            T[] val = base.ToArray();
            //
            OnBeforeListChanged(ListChangedType.ItemDeleted, val);
            //
            base.Clear();
            //
            OnListChanged(ListChangedType.ItemDeleted, val);
        }

        public new void Insert(int index, T item)
        {
            OnBeforeListChanged(ListChangedType.ItemAdded, new T[] { item });
            //
            base.Insert(index, item);
            //
            OnListChanged(ListChangedType.ItemAdded, new T[] { item });
        }

        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            var items = collection.ToArray();
            //
            OnBeforeListChanged(ListChangedType.ItemAdded, items);
            //
            base.InsertRange(index, items);
            //
            OnListChanged(ListChangedType.ItemAdded, items);
        }

        public new bool Remove(T item)
        {
            OnBeforeListChanged(ListChangedType.ItemDeleted, new T[] { item });
            //
            bool retval = base.Remove(item);
            //
            OnListChanged(ListChangedType.ItemDeleted, new T[] { item });
            //
            return retval;
        }

        public new int RemoveAll(Predicate<T> match)
        {
            List<T> list = base.FindAll(match);
            T[] val = list.ToArray();
            //
            OnBeforeListChanged(ListChangedType.ItemDeleted, val);
            //
            int retval = base.RemoveAll(match);
            //
            OnListChanged(ListChangedType.ItemDeleted, val);
            //
            return retval;
        }

        public new void RemoveAt(int index)
        {
            T val = base[index];
            //
            OnBeforeListChanged(ListChangedType.ItemDeleted, new T[] { val });
            //
            base.RemoveAt(index);
            //
            OnListChanged(ListChangedType.ItemDeleted, new T[] { val });
        }

        public new void RemoveRange(int index, int count)
        {
            T[] val = new T[count];
            for (int i = 0; i < count; i++)
                val[i] = base[index + i];
            //
            OnBeforeListChanged(ListChangedType.ItemDeleted, val);
            //
            base.RemoveRange(index, count);
            //
            OnListChanged(ListChangedType.ItemDeleted, val);
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace InfraManager.DataStructures.Collections
{
	[Serializable]
	public class Deque<T> : ICollection, IEnumerable, IEnumerable<T>
	{
		#region classes
		[Serializable]
		private class _Node
		{
			#region properties
			public T Item { get; private set; }

			public _Node Previous { get; set; }

			public _Node Next { get; set; }
			#endregion


			#region constructors
			public _Node(T item)
			{
				this.Item = item;
			} 
			#endregion
		}

		[Serializable]
		private class _Enumerator : IEnumerator<T>
		{
			#region fields
			private Deque<T> _owner;
			private _Node _currentItem;
			private T _current = default(T);
			private long _version;
			private bool _moveResult;
			private bool _isDisposed; 
			#endregion


			#region constructors
			public _Enumerator(Deque<T> owner)
			{
				_owner = owner;
				_currentItem = owner._front;
				_version = owner._version;
			} 
			#endregion


			#region interface IEnumerator
			object IEnumerator.Current
			{
				get
				{
					if (_isDisposed)
						throw new ObjectDisposedException(this.GetType().Name);
					else if (!_moveResult)
						throw new InvalidOperationException("The enumerator is positioned before the first element of the Deque or after the last element.");
					//
					return _current;
				}
			}

			public bool MoveNext()
			{
				if (_isDisposed)
					throw new ObjectDisposedException(this.GetType().Name);
				else if (_version != _owner._version)
					throw new InvalidOperationException("The Deque was modified after the enumerator was created.");
				//
				if (_currentItem == null)
				{
					_moveResult = false; 
				}
				else
				{
					_current = _currentItem.Item;
					_currentItem = _currentItem.Next;
					_moveResult = true;	
				}
				return _moveResult;
			}

			public void Reset()
			{
				if(_isDisposed)
                    throw new ObjectDisposedException(this.GetType().Name);
                else if(_version != _owner._version)
                    throw new InvalidOperationException("The Deque was modified after the enumerator was created.");
				//
                _currentItem = _owner._front;
                _moveResult = false;
			}
			#endregion


			#region interface IEnumerator<T>
			public T Current
			{
				get 
				{
					if (_isDisposed)
						throw new ObjectDisposedException(this.GetType().Name);
					else if (!_moveResult)
						throw new InvalidOperationException("The enumerator is positioned before the first element of the Deque or after the last element.");
					//
					return _current;
				}
			}
			#endregion


			#region interface IDisposable
			public void Dispose()
			{
				_isDisposed = true;
			}
			#endregion
		}
		#endregion


		#region fields
		private readonly object _lock = new object();
		private _Node _front;
		private _Node _back;
		private int _count;
		private long _version;
		#endregion


		#region constructors
		public Deque() { }

		public Deque(IEnumerable<T> collection)
		{
			if (collection == null)
				throw new ArgumentNullException("collection", "collection is null.");
			foreach (var item in collection)
				PushBack(item);
		} 
		#endregion


		#region interface ICollection
		public int Count { get { return _count; } }

		public bool IsSynchronized { get { return false; } }

		public object SyncRoot { get { return _lock; } }

		public void CopyTo(Array array, int index)
		{
			if (array == null)
				throw new ArgumentNullException("array", "array is null.");
			else if (array.Rank > 1)
				throw new ArgumentException("array is multidimentional.", "array");
			else if (index < 0)
				throw new ArgumentOutOfRangeException("index", "index is less than zero.");
			else if (index >= array.Length)
				throw new ArgumentException("index", "index is equal to or greater than the length of array.");
			else if (this.Count > array.Length - index)
				throw new ArgumentException("The number of elements in the source Deque is greater than the available space from index to the end of the destination array.");
			//
			var i = index;
			foreach (var item in this)
				array.SetValue(item, i++);
		}
		#endregion

		#region interface IEnumerable
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new _Enumerator(this);
		}
		#endregion

		#region interface IEnumerable<T>
		public virtual IEnumerator<T> GetEnumerator()
		{
			return new _Enumerator(this);
		}
		#endregion


		#region method Clear
		public void Clear()
		{
			_count = 0;
			_front = _back = null;
			_version++;
		} 
		#endregion

		#region method Contains
		public bool Contains(T item)
		{
			var equalityComparer = EqualityComparer<T>.Default;
			foreach (var item2 in this)
				if (equalityComparer.Equals(item, item2))
					return true;
			return false;
		}

		public bool Contains(T item, EqualityComparer<T> equalityComparer)
		{ 
			if (equalityComparer == null)
				throw new ArgumentNullException("equalityComparer", "equalityComparer is null.");
			//
			foreach (var item2 in this)
				if (equalityComparer.Equals(item, item2))
					return true;
			return false;
		}
		#endregion

		#region method PeekBack
		public T PeekBack()
		{
			if (this.Count == 0)
				throw new InvalidOperationException("Deque is empty.");
			//
			return _back.Item;
		}
		#endregion

		#region method PeekFront
		public T PeekFront()
		{
			if (this.Count == 0)
				throw new InvalidOperationException("Deque is empty.");
			//
			return _front.Item;
		}
		#endregion

		#region method PopBack
		public T PopBack()
		{
			if (this.Count == 0)
				throw new InvalidOperationException("Deque is empty.");
			//
			var value = _back.Item;
			_back = _back.Next;
			_count--;
			if (_count == 0)
			{
				_front = null;
			}
			else
			{
				_back.Previous = null;
			}
			_version++;
			return value;
		}
		#endregion

		#region method PopFront
		public T PopFront()
		{
			if (this.Count == 0)
				throw new InvalidOperationException("Deque is empty.");
			//
			var value = _front.Item;
			_front = _front.Previous;
			_count--;
			if (_count == 0)
			{
				_back = null;
			}
			else
			{
				_front.Next = null;
			}
			_version++;
			return value;
		}
		#endregion

		#region method PushBack
		public void PushBack(T item)
		{
			var node = new _Node(item);
			node.Next = _back;
			if (this.Count > 0)
				_back.Previous = node;
			_back = node;
			_count++;
			if (this.Count == 1)
				_front = node;
			_version++;
		} 
		#endregion

		#region method PushFront
		public void PushFront(T item)
		{
			var node = new _Node(item);
			node.Previous = _front;
			if (this.Count > 0)
				_front.Next = node;
			_front = node;
			_count++;
			if (this.Count == 1)
				_back = node;
			_version++;
		}
		#endregion

		#region method ToArray
		public T[] ToArray()
		{
			var array = new T[this.Count];
			var i = 0;
			foreach (var item in this)
				array[i++] = item;
			return array;
		}
		#endregion
	}
}

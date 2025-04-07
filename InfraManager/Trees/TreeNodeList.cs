using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace InfraManager.DataStructures.Trees
{
	public class TreeNodeList<T> : IList<TreeNode<T>>
	{
		#region fields
		private List<TreeNode<T>> _list = new List<TreeNode<T>>();
		#endregion


		#region properties
		public TreeNode<T> Parent { get; private set; }
		#endregion


		#region constructors
		internal TreeNodeList(TreeNode<T> parent)
		{
			#region assertions
			Debug.Assert(parent != null, "parent is null.");
			#endregion
			//
			this.Parent = parent;
		}
		#endregion


		#region interface IList
		public int Count { get { return _list.Count; } }

		public bool IsReadOnly { get { return false; } }

		public TreeNode<T> this[int index]
		{
			get { return _list[index]; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value", "value is null.");
				//
				_list[index].Parent = null;
				_list[index] = value;
			}
		}

		public void Add(TreeNode<T> item)
		{
			if (item == null)
				throw new ArgumentNullException("item", "item is null.");
			//
			_list.Add(item);
			item.Parent = this.Parent;
		}

		public void Clear()
		{
			_list.ForEach(x => x.Parent = null);
			_list.Clear();
		}

		public bool Contains(TreeNode<T> item)
		{
			if (item == null)
				throw new ArgumentNullException("item", "item is null.");
			//
			return _list.Contains(item);
		}

		public void CopyTo(TreeNode<T>[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}

		public IEnumerator<TreeNode<T>> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)_list).GetEnumerator();
		}

		public int IndexOf(TreeNode<T> item)
		{
			if (item == null)
				throw new ArgumentNullException("item", "item is null.");
			//
			return _list.IndexOf(item);
		}

		public void Insert(int index, TreeNode<T> item)
		{
			if (item == null)
				throw new ArgumentNullException("item", "item is null.");
			//
			_list.Insert(index, item);
			item.Parent = this.Parent;
		}

		public bool Remove(TreeNode<T> item)
		{
			if (item == null)
				throw new ArgumentNullException("item", "item is null.");
			//
			var result = _list.Remove(item);
			if (result)
				item.Parent = null;
			return result;
		}

		public void RemoveAt(int index)
		{
			var item = _list[index];
			_list.RemoveAt(index);
			item.Parent = null;
		}
		#endregion
	}
}

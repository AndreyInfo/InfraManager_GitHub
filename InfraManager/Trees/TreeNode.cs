
namespace InfraManager.DataStructures.Trees
{
	public class TreeNode<T>
	{
		#region fields
		private TreeNode<T> _parent;
		private TreeNodeList<T> _children;
		#endregion


		#region properties
		public T Value { get; set; }

		public TreeNode<T> Parent
		{
			get { return _parent; }
			set
			{
				if (object.ReferenceEquals(value, _parent))
					return;
				if (_parent != null)
					_parent.Children.Remove(this);
				if (value != null && !value.Children.Contains(this))
					value.Children.Add(this);
				_parent = value;
			}
		}

		public TreeNodeList<T> Children
		{
			get { return _children; }
		}
		#endregion


		#region constructors
		public TreeNode()
			: this(default(T))
		{ }
		

		public TreeNode(T value)
			: this(value, null)
		{ }

		public TreeNode(T value, TreeNode<T> parent)
		{
			this.Value = value;
			_parent = parent;
			_children = new TreeNodeList<T>(this);
		}
		#endregion
	}
}

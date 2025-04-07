using System;
using InfraManager.DataStructures.Graphs.Interfaces;

namespace InfraManager.DataStructures.Graphs
{
	public class TaggedEdge<TVertex, TTag> :
		Edge<TVertex>, ITagged<TTag>
	{
		private TTag _tag;


		public TaggedEdge(TVertex source, TVertex target, TTag tag)
			: base(source, target)
		{
			_tag = tag;
		}


		#region Events
		public event EventHandler TagChanged; 
		#endregion


		#region protected virtual method OnTagChanged
		protected virtual void OnTagChanged(EventArgs e)
		{
			var eventHandler = this.TagChanged;
			if (eventHandler != null)
				eventHandler(this, e);
		} 
		#endregion


		#region Properties
		public TTag Tag
		{
			get { return _tag; }
			set
			{
				if (object.Equals(_tag, value))
					return;
				_tag = value;
				OnTagChanged(EventArgs.Empty);
			}
		} 
		#endregion
	}
}

using System;
using System.Collections.Generic;

namespace InfraManager.DataStructures.Graphs.Collections
{
	public sealed class VertexList<TVertex> : List<TVertex>, ICloneable
	{
		public VertexList()
		{ }

		public VertexList(int capacity)
			: base(capacity)
		{ }

		public VertexList(VertexList<TVertex> other)
			: base(other)
		{ }


		#region method Clone
		public VertexList<TVertex> Clone()
		{
			return new VertexList<TVertex>(this);
		} 
		#endregion


		#region interface IClonable
		object ICloneable.Clone()
		{
			return this.Clone();
		} 
		#endregion
	}
}

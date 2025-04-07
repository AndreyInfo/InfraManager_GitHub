using System;
using System.Collections.Generic;
using InfraManager.DataStructures.Graphs.Interfaces;

namespace InfraManager.DataStructures.Graphs
{
	public sealed class EdgeList<TVertex, TEdge> : List<TEdge>, ICloneable where TEdge : IEdge<TVertex>
	{
		public EdgeList()
		{ }

		public EdgeList(int capacity)
			: base(capacity)
		{ }

		public EdgeList(EdgeList<TVertex, TEdge> list)
			: base(list)
		{ }


		#region method Clone
		public EdgeList<TVertex, TEdge> Clone()
		{
			return new EdgeList<TVertex, TEdge>(this);
		}
		#endregion


		#region IClonable
		object ICloneable.Clone()
		{
			return this.Clone();
		}
		#endregion
	}
}

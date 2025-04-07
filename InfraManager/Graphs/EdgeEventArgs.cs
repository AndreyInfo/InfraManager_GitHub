using System;
using System.Diagnostics;
using InfraManager.DataStructures.Graphs.Interfaces;

namespace InfraManager.DataStructures.Graphs
{
	public class EdgeEventArgs<TVertex, TEdge> : 
		EventArgs 
		where TEdge : IEdge<TVertex>
	{
		private readonly TEdge _edge;


		public EdgeEventArgs(TEdge edge)
		{
			Debug.Assert(edge != null);
			_edge = edge;
		}


		#region Properties
		public TEdge Edge { get { return _edge; } }

		public virtual TVertex Source { get { return _edge.Source; } }

		public virtual TVertex Target { get { return _edge.Target; } }
		#endregion
	}

	public delegate void EdgeAction<TVertex, TEdge>(object sender, EdgeEventArgs<TVertex, TEdge> edge) where TEdge : IEdge<TVertex>;
}

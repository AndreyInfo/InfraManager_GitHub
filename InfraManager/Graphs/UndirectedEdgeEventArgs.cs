using System;
using System.Diagnostics;
using InfraManager.DataStructures.Graphs.Interfaces;

namespace InfraManager.DataStructures.Graphs
{
	public class UndirectedEdgeEventArgs<TVertex, TEdge> :
		EdgeEventArgs<TVertex, TEdge>
		where TEdge : IEdge<TVertex>
	{
		private readonly bool _reversed;


		public UndirectedEdgeEventArgs(TEdge edge)
			: this(edge, false)
		{ }

		public UndirectedEdgeEventArgs(TEdge edge, bool reversed)
			: base(edge)
		{
			_reversed = reversed;
		}

		#region Properties
		public bool Reversed { get { return _reversed; } } 

		override public TVertex Source { get { return _reversed ? Edge.Target : Edge.Source; } }

		override public TVertex Target { get { return _reversed ? Edge.Source : Edge.Target; } }
		#endregion
	}

	public delegate void UndirectedEdgeAction<TVertex, TEdge>(object sender, UndirectedEdgeEventArgs<TVertex, TEdge> edge) where TEdge : IEdge<TVertex>;
}

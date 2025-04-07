using System;
using System.Diagnostics;

namespace InfraManager.DataStructures.Graphs
{
	public class VertextEventArgs<TVertex> : EventArgs
	{
		private readonly TVertex _vertex;


		public VertextEventArgs(TVertex vertex)
		{
			Debug.Assert(vertex != null);
			_vertex = vertex;
		}


		#region Properties
		public TVertex Vertex { get { return _vertex; } }
		#endregion
	}

	public delegate void VertexAction<TVertex>(TVertex vertex);
}

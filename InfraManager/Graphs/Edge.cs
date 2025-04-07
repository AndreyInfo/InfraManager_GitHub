using System.Diagnostics;
using InfraManager.DataStructures.Graphs.Interfaces;

namespace InfraManager.DataStructures.Graphs
{
	public class Edge<TVertex> : 
		IEdge<TVertex>
	{
		private readonly TVertex _source;
		private readonly TVertex _target;


		public Edge(TVertex source, TVertex target)
		{
			Debug.Assert(source != null);
			Debug.Assert(target != null);

			_source = source;
			_target = target;
		}


		#region Properties
		public TVertex Source { get { return _source; } }

		public TVertex Target { get { return _target; } }
		#endregion
	}
}

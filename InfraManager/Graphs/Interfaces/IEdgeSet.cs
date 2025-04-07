using System.Collections.Generic;

namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IEdgeSet<TVertex, TEdge>
		where TEdge : IEdge<TVertex>
	{
		bool ContainsEdge(TEdge edge);

		IEnumerable<TEdge> GetEdges();


		bool IsEdgesEmpty { get; }

		int EdgesCount { get; }
	}
}

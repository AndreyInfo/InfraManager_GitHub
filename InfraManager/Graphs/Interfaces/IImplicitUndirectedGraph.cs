using System.Collections.Generic;

namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IImplicitUndirectedGraph<TVertex, TEdge> : 
		IGraph<TVertex, TEdge>, IIncidenceGraph<TVertex, TEdge>
		where TEdge : IEdge<TVertex>
	{
		bool ContainsVertex(TVertex vertex);

		bool IsAdjacentEdgesEmpty(TVertex vertex);

		int GetAdjacentDegree(TVertex vertex);

		IEnumerable<TEdge> GetAdjacentEdges(TVertex vertex);

		bool TryGetAdjacentEdges(TVertex vertex, out IEnumerable<TEdge> edges);

		TEdge GetAdjacentEdge(TVertex vertex, int index);


		EdgeEqualityComparer<TVertex, TEdge> EdgeEqualityComparer { get; }
	}
}

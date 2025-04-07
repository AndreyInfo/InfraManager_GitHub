using System.Collections.Generic;

namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IImplicitBidirectedGraph<TVertex, TEdge> :
		IImplicitDirectedGraph<TVertex, TEdge>, IIncidenceGraph<TVertex, TEdge>
		where TEdge : IEdge<TVertex>
	{
		bool IsInEdgesEmpty(TVertex vertex);

		int GetInDegree(TVertex vertex);

		int GetDegree(TVertex vertex);

		IEnumerable<TEdge> GetInEdges(TVertex vertex);

		bool TryGetInEdges(TVertex vertex, out IEnumerable<TEdge> edges);

		TEdge GetInEdge(TVertex vertex, int index);
	}
}

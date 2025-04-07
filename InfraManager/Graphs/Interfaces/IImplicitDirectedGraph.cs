using System.Collections.Generic;

namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IImplicitDirectedGraph<TVertex, TEdge> : 
		IGraph<TVertex, TEdge>
		where TEdge : IEdge<TVertex>
	{
		bool ContainsVertex(TVertex vertex);

		bool IsOutEdgesEmpty(TVertex vertex);

		int GetOutDegree(TVertex vertex);

		IEnumerable<TEdge> GetOutEdges(TVertex vertex);

		bool TryGetOutEdges(TVertex vertex, out IEnumerable<TEdge> edges);

		TEdge GetOutEdge(TVertex vertex, int index);
	}
}

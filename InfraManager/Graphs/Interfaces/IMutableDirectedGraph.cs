using System.Collections.Generic;

namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IMutableDirectedGraph<TVertex, TEdge> :
		IDirectedGraph<TVertex, TEdge>, IMutableVertexAndEdgeListGraph<TVertex, TEdge>
		where TEdge : IEdge<TVertex>
	{
		void RemoveOutEdges(TVertex vertex);

		int RemoveOutEdgeIf(TVertex vertex, EdgePredicate<TVertex, TEdge> predicate);
	}
}

using System.Collections.Generic;

namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IMutableBidirectedGraph<TVertex, TEdge> :
		IBidirectedGraph<TVertex, TEdge>, IMutableDirectedGraph<TVertex, TEdge>
		where TEdge : IEdge<TVertex>
	{
		void RemoveEdges(TVertex vertex);

		void RemoveInEdges(TVertex vertex);

		int RemoveInEdgeIf(TVertex vertex, EdgePredicate<TVertex, TEdge> predicate);
	}
}

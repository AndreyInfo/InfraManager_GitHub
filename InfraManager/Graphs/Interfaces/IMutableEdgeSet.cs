using System.Collections.Generic;

namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IMutableEdgeSet<TVertex, TEdge> :
		IEdgeSet<TVertex, TEdge>
		where TEdge : IEdge<TVertex>
	{
		event EdgeAction<TVertex, TEdge> EdgeAdded;

		event EdgeAction<TVertex, TEdge> EdgeRemoved;


		bool AddEdge(TEdge edge);

		int AddEdges(IEnumerable<TEdge> edges);

		bool RemoveEdge(TEdge edge);

		int RemoveEdgeIf(EdgePredicate<TVertex, TEdge> predicate);
	}
}

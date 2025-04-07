using System.Collections.Generic;

namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IMutableVertexAndEdgeSet<TVertex, TEdge> :
		IMutableVertexSet<TVertex>, IMutableEdgeSet<TVertex, TEdge>
		where TEdge : IEdge<TVertex>
	{
		bool AddVerticesAndEdge(TEdge edge);

		int AddVerticesAndEdges(IEnumerable<TEdge> edges);
	}
}

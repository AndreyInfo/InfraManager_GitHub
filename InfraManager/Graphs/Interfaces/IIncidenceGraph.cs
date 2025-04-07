using System.Collections.Generic;

namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IIncidenceGraph<TVertex, TEdge> : 
		IGraph<TVertex, TEdge> 
		where TEdge : IEdge<TVertex>
	{
		bool ContainsEdge(TVertex source, TVertex target);

		bool TryGetEdge(TVertex source, TVertex target, out TEdge edge);

		bool TryGetEdges(TVertex source, TVertex target, out IEnumerable<TEdge> edges);
	}
}


namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IMutableUndirectedGraph<TVertex, TEdge> :
		IUndirectedGraph<TVertex, TEdge>, IMutableVertexAndEdgeListGraph<TVertex, TEdge>
		where TEdge : IEdge<TVertex>
	{
		void RemoveAdjacentEdges(TVertex vertex);

		int RemoveAdjacentEdgeIf(TVertex vertex, EdgePredicate<TVertex, TEdge> predicate);
	}
}

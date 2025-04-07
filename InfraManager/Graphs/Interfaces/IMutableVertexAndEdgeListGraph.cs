
namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IMutableVertexAndEdgeListGraph<TVertex, TEdge> :
		IMutableVertexListGraph<TVertex, TEdge>, IMutableEdgeListGraph<TVertex, TEdge>, IMutableVertexAndEdgeSet<TVertex, TEdge>
		where TEdge : IEdge<TVertex>
	{ }
}

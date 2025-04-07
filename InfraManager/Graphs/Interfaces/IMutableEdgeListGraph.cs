
namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IMutableEdgeListGraph<TVertex, TEdge> :
		IMutableGraph<TVertex, TEdge>, IEdgeListGraph<TVertex, TEdge>, IMutableEdgeSet<TVertex, TEdge>
		where TEdge : IEdge<TVertex>
	{ }
}

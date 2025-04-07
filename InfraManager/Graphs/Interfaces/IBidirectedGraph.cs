
namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IBidirectedGraph<TVertex, TEdge> :
		IImplicitBidirectedGraph<TVertex, TEdge>, IVertexAndEdgeListGraph<TVertex, TEdge>
		where TEdge : IEdge<TVertex>
	{ }
}


namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IDirectedGraph<TVertex, TEdge> :
		IImplicitDirectedGraph<TVertex, TEdge>, IVertexAndEdgeListGraph<TVertex, TEdge>
		where TEdge : IEdge<TVertex>
	{ }
}

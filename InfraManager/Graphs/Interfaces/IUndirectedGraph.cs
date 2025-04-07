
namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IUndirectedGraph<TVertex, TEdge> :
		IImplicitUndirectedGraph<TVertex, TEdge>, IVertexAndEdgeListGraph<TVertex, TEdge>
		where TEdge : IEdge<TVertex>
	{ }
}

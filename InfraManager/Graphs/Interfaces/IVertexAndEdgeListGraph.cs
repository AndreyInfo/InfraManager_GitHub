
namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IVertexAndEdgeListGraph<TVertex, TEdge> :
		IVertexListGraph<TVertex, TEdge>, IEdgeListGraph<TVertex, TEdge>
		where TEdge : IEdge<TVertex>
	{ }
}

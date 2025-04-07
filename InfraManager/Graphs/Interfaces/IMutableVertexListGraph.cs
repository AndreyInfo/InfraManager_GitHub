
namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IMutableVertexListGraph<TVertex, TEdge> :
		IMutableGraph<TVertex, TEdge>, IVertexListGraph<TVertex, TEdge>, IMutableVertexSet<TVertex>
		where TEdge : IEdge<TVertex>
	{ }
}

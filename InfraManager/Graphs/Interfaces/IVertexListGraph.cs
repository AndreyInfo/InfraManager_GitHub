
namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IVertexListGraph<TVertex, TEdge> :
		IGraph<TVertex, TEdge>, IVertexSet<TVertex>
		where TEdge : IEdge<TVertex>
	{ }
}

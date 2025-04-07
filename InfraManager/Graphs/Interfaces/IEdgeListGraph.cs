
namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IEdgeListGraph<TVertex, TEdge> :
		IGraph<TVertex, TEdge>, IEdgeSet<TVertex, TEdge> 
		where TEdge : IEdge<TVertex>
	{ }
}

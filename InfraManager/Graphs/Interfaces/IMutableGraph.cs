
namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IMutableGraph<TVertex, TEdge> :
		IGraph<TVertex, TEdge>
		where TEdge : IEdge<TVertex>
	{
		void Clear();
	}
}

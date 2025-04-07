
namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IGraph<TVertex, TEdge> 
		where TEdge : IEdge<TVertex>
	{
		bool IsDirected { get; }

		bool IsParallelEdgesAllowed { get; }
	}
}

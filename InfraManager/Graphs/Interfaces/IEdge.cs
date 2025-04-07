
namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IEdge<TVertex>
	{
		TVertex Source { get; }

		TVertex Target { get; }
	}
}

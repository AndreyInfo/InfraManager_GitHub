using InfraManager.DataStructures.Graphs.Interfaces;

namespace InfraManager.DataStructures.Graphs
{
	public delegate TEdge EdgeFactory<TVertex, TEdge>(TVertex source, TVertex target) where TEdge : IEdge<TVertex>;
}

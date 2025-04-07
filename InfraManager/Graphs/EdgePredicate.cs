using InfraManager.DataStructures.Graphs.Interfaces;

namespace InfraManager.DataStructures.Graphs
{
	public delegate bool EdgePredicate<TVertex, TEdge>(TEdge edge) where TEdge : IEdge<TVertex>;
}

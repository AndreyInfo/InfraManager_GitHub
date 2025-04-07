using System.Collections.Generic;

namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IMutableVertexSet<TVertex> : 
		IVertexSet<TVertex>
	{
		event VertexAction<TVertex> VertexAdded;

		event VertexAction<TVertex> VertexRemoved; 


		bool AddVertex(TVertex vertex);

		int AddVertices(IEnumerable<TVertex> vertices);

		bool RemoveVertex(TVertex vertex);

		int RemoveVertexIf(VertexPredicate<TVertex> predicate);
	}
}

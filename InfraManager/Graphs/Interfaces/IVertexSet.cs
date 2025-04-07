using System.Collections.Generic;

namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface IVertexSet<TVertex>
	{
		bool ContainsVertex(TVertex vertex);

		IEnumerable<TVertex> GetVerticies();


		bool IsVerticesEmpty { get; }

		int VerticesCount { get; }
	}
}

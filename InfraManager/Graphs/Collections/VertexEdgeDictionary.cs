using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using InfraManager.DataStructures.Graphs.Interfaces;

namespace InfraManager.DataStructures.Graphs
{
	[Serializable]
	public class VertexEdgeDictionary<TVertex, TEdge> : Dictionary<TVertex, EdgeList<TVertex, TEdge>>, ICloneable where TEdge : IEdge<TVertex>
	{
		public VertexEdgeDictionary()
		{ }

		public VertexEdgeDictionary(int capacity)
			: base(capacity)
		{ }

		public VertexEdgeDictionary(SerializationInfo info, StreamingContext context)
			: base(info, context) { }


		#region method Clone
		public VertexEdgeDictionary<TVertex, TEdge> Clone()
		{
			VertexEdgeDictionary<TVertex, TEdge> result = new VertexEdgeDictionary<TVertex, TEdge>(this.Count);
			foreach (var pair in this)
				result.Add(pair.Key, pair.Value.Clone());
			return result;
		}
		#endregion


		#region interface ICloneable
		object ICloneable.Clone()
		{
			return this.Clone();
		}
		#endregion
	}
}

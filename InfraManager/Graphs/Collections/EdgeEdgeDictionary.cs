using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using InfraManager.DataStructures.Graphs.Interfaces;

namespace InfraManager.DataStructures.Graphs.Collections
{
	[Serializable]
	public class EdgeEdgeDictionary<TVertex, TEdge> : Dictionary<TEdge, TEdge>, ICloneable where TEdge : IEdge<TVertex>
	{
		public EdgeEdgeDictionary()
		{ }

		public EdgeEdgeDictionary(int capacity)
			: base(capacity)
		{ }

		protected EdgeEdgeDictionary(SerializationInfo info, StreamingContext context)
			: base(info, context) { }


		#region method Clone
		public EdgeEdgeDictionary<TVertex, TEdge> Clone()
		{
			EdgeEdgeDictionary<TVertex, TEdge> result = new EdgeEdgeDictionary<TVertex, TEdge>(this.Count);
			foreach (var pair in this)
				result.Add(pair.Key, pair.Value);
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

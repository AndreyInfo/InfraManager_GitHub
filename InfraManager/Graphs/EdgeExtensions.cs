using System;
using System.Linq;
using System.Collections.Generic;
using InfraManager.DataStructures.Graphs.Interfaces;

namespace InfraManager.DataStructures.Graphs
{
	public static class EdgeExtensions
	{
		#region IsSelfEdge
		public static bool IsSelfEdge<TVertex, TEdge>(this TEdge edge)
			where TEdge : IEdge<TVertex>
		{
			return edge.Source.Equals(edge.Target);
		}
		#endregion

		#region GetOtherVertex
		public static TVertex GetOtherVertex<TVertex, TEdge>(this TEdge edge, TVertex vertex)
			where TEdge : IEdge<TVertex>
		{
			return edge.Source.Equals(vertex) ? edge.Target : edge.Source;
		}
		#endregion

		#region IsAdjacent
		public static bool IsAdjacent<TVertex, TEdge>(this TEdge edge, TVertex vertex)
			where TEdge : IEdge<TVertex>
		{
			return edge.Source.Equals(vertex) || edge.Target.Equals(vertex);
		}
		#endregion

		#region IsPath
		public static bool IsPath<TVertex, TEdge>(this IEnumerable<TEdge> edges)
			where TEdge : IEdge<TVertex>
		{
			bool first = true;
			TVertex lastTarget = default(TVertex);
			foreach (var edge in edges)
			{
				if (first)
				{
					lastTarget = edge.Target;
					first = false;
				}
				else
				{
					if (!lastTarget.Equals(edge.Source))
						return false;
					lastTarget = edge.Target;
				}
			}
			return true;
		}
		#endregion

		#region IsPathWithCycles
		public static bool IsPathWithCycles<TVertex, TEdge>(this IEnumerable<TEdge> edges)
			where TEdge : IEdge<TVertex>
		{
			var vertices = new Dictionary<TVertex, int>();
			bool first = true;
			foreach (var edge in edges)
			{
				if (first)
				{
					if (edge.IsSelfEdge<TVertex, TEdge>())
						return true;
					vertices.Add(edge.Source, 0);
					vertices.Add(edge.Target, 0);
					first = false;
				}
				else
				{
					if (vertices.ContainsKey(edge.Target))
						return true;
					vertices.Add(edge.Target, 0);
				}
			}
			return false;
		}
		#endregion

		#region IsPathWithoutCycles
		public static bool IsPathWithoutCycles<TVertex, TEdge>(this IEnumerable<TEdge> edges)
			where TEdge : IEdge<TVertex>
		{
			var vertices = new Dictionary<TVertex, int>();
			bool first = true;
			TVertex lastTarget = default(TVertex);
			foreach (var edge in edges)
			{
				if (first)
				{
					if (edge.IsSelfEdge<TVertex, TEdge>())
						return false;
					vertices.Add(edge.Source, 0);
					vertices.Add(edge.Target, 0);
					lastTarget = edge.Target;
					first = false;
				}
				else
				{
					if (!lastTarget.Equals(edge.Source))
						return false;
					if (vertices.ContainsKey(edge.Target))
						return false;
					lastTarget = edge.Target;
					vertices.Add(edge.Target, 0);
				}
			}
			return true;
		}
		#endregion

		#region IsPredecessor
		public static bool IsPredecessor<TVertex, TEdge>(this IDictionary<TVertex, TEdge> predecessors, TVertex root, TVertex vertex)
			where TEdge : IEdge<TVertex>
		{
			TVertex predecessor = vertex;
			if (root.Equals(predecessor))
				return true;
			TEdge edge;
			while (predecessors.TryGetValue(predecessor, out edge))
			{
				var source = edge.GetOtherVertex(predecessor);
				if (predecessor.Equals(source))
					return false;
				predecessor = source;
				if (root.Equals(predecessor))
					return true;
			}
			return false;
		}
		#endregion

		#region TryGetPath
		public static bool TryGetPath<TVertex, TEdge>(this IDictionary<TVertex, TEdge> predecessors, TVertex vertex, out IEnumerable<TEdge> result)
			where TEdge : IEdge<TVertex>
		{
			var path = new List<TEdge>();
			TVertex predecessor = vertex;
			TEdge edge;
			while (predecessors.TryGetValue(predecessor, out edge))
			{
				var source = edge.GetOtherVertex(predecessor);
				if (predecessor.Equals(source))
				{
					result = null;
					return false;
				}
				path.Add(edge);
				predecessor = source;
			}
			if (path.Count > 0)
			{
				path.Reverse();
				result = path.ToArray();
				return true;
			}
			else
			{
				result = null;
				return false;
			}
		}
		#endregion


		#region method GetVertexEquality
		public static EdgeEqualityComparer<TVertex, TEdge> GetVertexEquality<TVertex, TEdge>()
			where TEdge : IEdge<TVertex>
		{
			if (typeof(IUndirectedEdge<TVertex>).IsAssignableFrom(typeof(TEdge)))
				return new EdgeEqualityComparer<TVertex, TEdge>(UndirectedVertexEquality<TVertex, TEdge>);
			else
				return new EdgeEqualityComparer<TVertex, TEdge>(DirectedVertexEquality<TVertex, TEdge>);
		}
		#endregion

		#region method UndirectedVertexEquality
		public static bool UndirectedVertexEquality<TVertex, TEdge>(this TEdge edge, TVertex source, TVertex target)
			where TEdge : IEdge<TVertex>
		{
			return (edge.Source.Equals(source) && edge.Target.Equals(target)) ||
				(edge.Target.Equals(source) && edge.Source.Equals(target));
		}
		#endregion

		#region method DirectedVertexEquality
		public static bool DirectedVertexEquality<TVertex, TEdge>(this TEdge edge, TVertex source, TVertex target)
			where TEdge : IEdge<TVertex>
		{
			return edge.Source.Equals(source) && edge.Target.Equals(target);
		}
		#endregion
	}
}

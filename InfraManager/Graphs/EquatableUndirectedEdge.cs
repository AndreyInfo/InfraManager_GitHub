using System;
using InfraManager.DataStructures.Graphs.Interfaces;

namespace InfraManager.DataStructures.Graphs
{
	public class EquatableUndirectedEdge<TVertex> : UndirectedEdge<TVertex>, IEquatable<EquatableUndirectedEdge<TVertex>>
	{
		public EquatableUndirectedEdge(TVertex source, TVertex target)
			: base(source, target)
		{ }


		#region override method Equals
		public override bool Equals(object obj)
		{
			EquatableUndirectedEdge<TVertex> other;
			return (other = obj as EquatableUndirectedEdge<TVertex>) != null && this.Equals(other);
		} 
		#endregion

		#region override method GetHashCode
		public override int GetHashCode()
		{
			return HashCodeUtilities.Combine(
				this.Source.GetHashCode(),
				this.Target.GetHashCode());
		}
		#endregion


		#region interface IEquatable
		public bool Equals(EquatableUndirectedEdge<TVertex> other)
		{
			return 
				(this.Source.Equals(other.Source) && this.Target.Equals(other.Target)) ||
				(this.Source.Equals(other.Target) && this.Target.Equals(other.Source));
		}
		#endregion
	}
}

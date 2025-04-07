using System;
using InfraManager.DataStructures.Graphs.Interfaces;

namespace InfraManager.DataStructures.Graphs
{
	public class EquatableEdge<TVertex> : Edge<TVertex>, IEquatable<EquatableEdge<TVertex>>
	{
		public EquatableEdge(TVertex source, TVertex target)
			: base(source, target)
		{ }


		#region override method Equals
		public override bool Equals(object obj)
		{
			EquatableEdge<TVertex> other;
			return (other = obj as EquatableEdge<TVertex>) != null && this.Equals(other);
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
		public bool Equals(EquatableEdge<TVertex> other)
		{
			return this.Source.Equals(other.Source) && this.Target.Equals(other.Target);
		}
		#endregion
	}
}

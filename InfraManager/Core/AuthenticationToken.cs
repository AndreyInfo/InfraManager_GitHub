using System;
using InfraManager.ComponentModel;
using InfraManager.Core.Helpers;

namespace InfraManager.Core
{
	[Serializable]
	public sealed class AuthenticationToken : ISummaryProvider
	{
		#region properties
		public object Identity { get; private set; }

		public AuthenticationType AuthenticationType { get; private set; }
		#endregion


		#region constructors
		public AuthenticationToken(object identity, AuthenticationType authenticationType)
		{
			if (identity == null)
				throw new ArgumentNullException("identity", "identity is null.");
			//
			this.Identity = identity;
			this.AuthenticationType = authenticationType;
		} 
		#endregion


		#region interface ISummaryProvider
		object[] ISummaryProvider.GetUniqueVector()
		{
			return new object[] { this.Identity, this.AuthenticationType };
		}

		string ISummaryProvider.ToString()
		{
			return string.Format("{{identity: '{0}', authentication type: '{1}'}}", this.Identity, this.AuthenticationType);
		}
		#endregion


		#region method Equals
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (object.ReferenceEquals(this, obj)) return true;
			if (this.GetType() != obj.GetType()) return false;
			AuthenticationToken other = (AuthenticationToken)obj;
			return this.Identity.Equals(other.Identity) && this.AuthenticationType == other.AuthenticationType;
		}
		#endregion

		#region override method GetHashCode
		public override int GetHashCode()
		{
			return HashCodeHelper.GetBernsteinHashCode(this.Identity, this.AuthenticationType);
		}
		#endregion


		#region operators
		public static bool operator ==(AuthenticationToken x, AuthenticationToken y)
		{
			if (object.ReferenceEquals(x, y)) return true;
			if ((object)x == null || (object)y == null) return false;
			return x.Identity.Equals(y.Identity) && x.AuthenticationType == y.AuthenticationType;
		}

		public static bool operator !=(AuthenticationToken x, AuthenticationToken y)
		{
				return !(x == y);
		}
		#endregion
	}
}

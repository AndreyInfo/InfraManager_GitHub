using System;
using InfraManager.Core.Extensions;
using System.Collections.Generic;

namespace InfraManager.Core.Logging
{
	[Serializable]
	public sealed class SeverityLevel : IComparable<SeverityLevel>, IComparable
	{
		#region fields
		public static readonly SeverityLevel All = new SeverityLevel(byte.MinValue, "all");
		public static readonly SeverityLevel Verbose = new SeverityLevel(10, "verbose");
		public static readonly SeverityLevel Trace = new SeverityLevel(20, "trace");
		public static readonly SeverityLevel Debug = new SeverityLevel(30, "debug");
		public static readonly SeverityLevel Info = new SeverityLevel(40, "info");
		public static readonly SeverityLevel Warning = new SeverityLevel(50, "warning");
		public static readonly SeverityLevel Error = new SeverityLevel(60, "error");
		public static readonly SeverityLevel Critical = new SeverityLevel(70, "critical");
		public static readonly SeverityLevel Fatal = new SeverityLevel(80, "fatal");
		public static readonly SeverityLevel None = new SeverityLevel(byte.MaxValue, "none");
		private static readonly Dictionary<string, SeverityLevel> _severityLevelsDictionary =
			new Dictionary<string, SeverityLevel>(StringComparer.InvariantCultureIgnoreCase)
			{ 
				{ All.Name, All },
				{ Verbose.Name, Verbose },
				{ Trace.Name, Trace },
				{ Debug.Name, Debug },
				{ Info.Name, Info },
				{ Warning.Name, Warning },
				{ Error.Name, Error },
				{ Critical.Name, Critical },
				{ Fatal.Name, Fatal },
				{ None.Name, None },
			};
		#endregion


		#region properties
		public byte Level { get; private set; }

		public string Name { get; private set; }

		public string DisplayName { get; private set; }
		#endregion


		#region constructors
		private SeverityLevel(byte level, string name)
			: this(level, name, name)
		{ }

		private SeverityLevel(byte level, string name, string displayName)
		{
			if (name.IsNullOrEmpty())
				throw new ArgumentException("name is null or empty.", "name");
			if (displayName.IsNullOrEmpty())
				throw new ArgumentException("displayName is null or empty.", "displayName");
			//
			this.Level = level;
			this.Name = name;
			this.DisplayName = displayName;
		}
		#endregion


		#region interface IComparable
		public int CompareTo(SeverityLevel obj)
		{
			return this.Level.CompareTo(obj.Level);
		}

		int IComparable.CompareTo(object obj)
		{
			SeverityLevel other = obj as SeverityLevel;
			if (other == null) throw new ArgumentException("Object is of different type.");
			return CompareTo(other);
		}
		#endregion


		#region static method GetSeverityLevel
		public static SeverityLevel GetSeverityLevel(string name)
		{
			SeverityLevel severityLevel;
			if (_severityLevelsDictionary.TryGetValue(name, out severityLevel))
				return severityLevel;
			return null;
		}
		#endregion

		#region override method ToString
		public override string ToString()
		{
			return this.Name;
		}
		#endregion

		#region override method Equals
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (object.ReferenceEquals(this, obj)) return true;
			if (this.GetType() != obj.GetType()) return false;
			SeverityLevel other = (SeverityLevel)obj;
			return other.Level == this.Level;
		}
		#endregion

		#region override method GetHashCode
		public override int GetHashCode()
		{
			return this.Level;
		}
		#endregion


		#region operators
		public static bool operator ==(SeverityLevel x, SeverityLevel y)
		{
			if (object.ReferenceEquals(x, y)) return true;
			if ((object)x == null || (object)y == null) return false;
			return x.Level == y.Level;
		}

		public static bool operator !=(SeverityLevel x, SeverityLevel y)
		{
			return !(x == y);
		}

		public static bool operator <(SeverityLevel x, SeverityLevel y)
		{
			return x.Level < y.Level;
		}

		public static bool operator >(SeverityLevel x, SeverityLevel y)
		{
			return x.Level > y.Level;
		}

		public static bool operator <=(SeverityLevel x, SeverityLevel y)
		{
			return x.Level <= y.Level;
		}

		public static bool operator >=(SeverityLevel x, SeverityLevel y)
		{
			return x.Level >= y.Level;
		}
		#endregion
	}
}

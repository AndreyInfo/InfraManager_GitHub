using System;
using System.Runtime.Serialization;

namespace InfraManager.Core.Exceptions
{
	[Serializable]
	public class ObjectInUseException : ApplicationException
	{
		#region properties
		public Guid ObjectID { get; private set; }

		public string ObjectName { get; private set; }

		public override string Message
		{
			get
			{
                if (string.IsNullOrWhiteSpace(this.ObjectName))
                    this.ObjectName = base.Message ?? string.Empty;
                //
				var friendlyClassName = Helpers.TypeHelper.GetFriendlyClassName(TargetSite.DeclaringType);
				if (string.IsNullOrEmpty(friendlyClassName))
				{
					if (string.IsNullOrEmpty(this.ObjectName))
						return "Объект используется.";
					else
						return string.Format("Объект '{0}' используется.", this.ObjectName);
				}
				else
				{
					if (string.IsNullOrEmpty(this.ObjectName))
						return string.Format("Объект [{0}] используется.", friendlyClassName);
					else
						return string.Format("Объект '{0}' [{1}] используется.", this.ObjectName, friendlyClassName);
				}
			}
		}
		#endregion


		#region constructors
		public ObjectInUseException(Guid objectID)
			: this(objectID, null)
		{ }

		public ObjectInUseException(Guid objectID, string objectName)
			: base()
		{
			this.ObjectID = objectID;
			this.ObjectName = objectName;
		}

		public ObjectInUseException()
			: base("Объект используется.")
		{ }

		public ObjectInUseException(string message)
			: base(message)
		{ }

		public ObjectInUseException(string message, Exception innerException)
			: base(message, innerException)
		{ }

		public ObjectInUseException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
		#endregion
	}
}

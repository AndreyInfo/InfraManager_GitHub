using System;
using System.Reflection;
using System.Runtime.Serialization;


namespace InfraManager.Core.Exceptions
{
	[Serializable]
	public sealed class ObjectDeletedException : ApplicationException
	{
		#region properties
		public Guid ObjectID {get; private set;}
		
		public string ObjectName {get; private set;}
		
		public override string Message
		{
			get
			{
				var friendlyClassName = Helpers.TypeHelper.GetFriendlyClassName(TargetSite.DeclaringType);
				if (string.IsNullOrEmpty(friendlyClassName))
				{
					if (string.IsNullOrEmpty(this.ObjectName))
						return "Объект был удален другим пользователем.";
					else
						return string.Format("Объект '{0}' был удален другим пользователем.", this.ObjectName);
				}
				else
				{
					if (string.IsNullOrEmpty(this.ObjectName))
						return string.Format("Объект [{0}] был удален другим пользователем.", friendlyClassName);
					else
						return string.Format("Объект '{0}' [{1}] был удален другим пользователем.", this.ObjectName, friendlyClassName);
				}
			}
		}
		#endregion


		#region constructors
		public ObjectDeletedException(Guid objectID)
			: this(objectID, null)
		{ }

		public ObjectDeletedException(Guid objectID, string objectName)
			: base()
		{
			this.ObjectID = objectID;
			this.ObjectName = objectName;
		}

		public ObjectDeletedException(string message)
			: base(message)
		{ }

		public ObjectDeletedException(string message, Exception innerException)
			: base(message, innerException)
		{ }

		public ObjectDeletedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ } 
		#endregion
	}
}

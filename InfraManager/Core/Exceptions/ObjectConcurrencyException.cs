using System;
using System.Runtime.Serialization;

namespace InfraManager.Core.Exceptions
{
	[Serializable]
	public class ObjectConcurrencyException : ApplicationException
	{
		#region properties
		public Guid ObjectID { get; private set; }

		public string ObjectName { get; private set; }

		public override string Message
		{
			get
			{
				var friendlyClassName = Helpers.TypeHelper.GetFriendlyClassName(TargetSite.DeclaringType);
				if (string.IsNullOrEmpty(friendlyClassName))
				{
					if (string.IsNullOrEmpty(this.ObjectName))
						return "Объект был изменен или удален другим пользователем.";
					else
						return string.Format("Объект '{0}' был изменен или удален другим пользователем.", this.ObjectName);
				}
				else
				{
					if (string.IsNullOrEmpty(this.ObjectName))
						return string.Format("Объект [{0}] был изменен или удален другим пользователем.", friendlyClassName);
					else
						return string.Format("Объект '{0}' [{1}] был изменен или удален другим пользователем.", this.ObjectName, friendlyClassName);
				}

			}
		}
		#endregion


		#region constructors
		public ObjectConcurrencyException(Guid objectID)
			: this(objectID, null)
		{ }

		public ObjectConcurrencyException(Guid objectID, string objectName)
			: base()
		{
			this.ObjectID = objectID;
			this.ObjectName = objectName;
		}

		public ObjectConcurrencyException()
			: base("Объект был изменен или удален другим пользователем.")
		{ }

		public ObjectConcurrencyException(string message)
			: base(message)
		{ }

		public ObjectConcurrencyException(string message, Exception innerException)
			: base(message, innerException)
		{ }

		public ObjectConcurrencyException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
		#endregion
	}
}

using System;
using System.Runtime.Serialization;

namespace InfraManager.Core.Exceptions
{
	//
	//TODO: hueta
	//
	[Serializable]
	public sealed class ObjectConstraintException : ApplicationException
	{
		#region properties
		public Type ObjectType { get; private set; }

		public Guid ObjectID { get; private set; }

		public string ObjectName { get; private set; }

		public ObjectConstraintReason Reason { get; private set; }

		public override string Message
		{
			get
			{
				var friendlyClassName = this.ObjectType == null ? Helpers.TypeHelper.GetFriendlyClassName(TargetSite.DeclaringType) : Helpers.TypeHelper.GetFriendlyClassName(this.ObjectType);
				//
				switch (this.Reason)
				{
					case ObjectConstraintReason.NotExists:
						if (string.IsNullOrEmpty(friendlyClassName))
						{
							if (string.IsNullOrEmpty(this.ObjectName))
								return "Невозможно создать связь. Объект был удален другим пользователем.";
							else
								return string.Format("Невозможно создать связь. Объект '{0}' был удален другим пользователем.", this.ObjectName);
						}
						else
						{
							if (string.IsNullOrEmpty(this.ObjectName))
								return string.Format("Невозможно создать связь. Объект [{0}] был удален другим пользователем.", friendlyClassName);
							else
								return string.Format("Невозможно создать связь. Объект '{0}' [{1}] был удален другим пользователем.", this.ObjectName, friendlyClassName);
						}
					case ObjectConstraintReason.ChildExists:
						if (string.IsNullOrEmpty(friendlyClassName))
						{
							if (string.IsNullOrEmpty(this.ObjectName))
								return "Существуют дочерние объекты.";
							else
								return string.Format("Объект '{0}' содержит дочерние объекты.", this.ObjectName);
						}
						else
						{
							if (string.IsNullOrEmpty(this.ObjectName))
								return string.Format("Существуют дочерние объекты [{0}].", friendlyClassName);
							else
								return string.Format("Объект '{0}' [{1}] содержит дочерние объекты.", this.ObjectName, friendlyClassName);
						}
					case ObjectConstraintReason.ParentNotExists:
						if (string.IsNullOrEmpty(friendlyClassName))
						{
							if (string.IsNullOrEmpty(this.ObjectName))
								return "Невозможно создать связь. Родительский объект был удален другим пользователем.";
							else
								return string.Format("Невозможно создать связь. Родительский объект '{0}' был удален другим пользователем.", this.ObjectName);
						}
						else
						{
							if (string.IsNullOrEmpty(this.ObjectName))
								return string.Format("Невозможно создать связь. Родительский объект [{0}] был удален другим пользователем.", friendlyClassName);
							else
								return string.Format("Невозможно создать связь. Родительский объект '{0}' [{1}] был удален другим пользователем.", this.ObjectName, friendlyClassName);
						}
					case ObjectConstraintReason.ParentExists:
						if (string.IsNullOrEmpty(friendlyClassName))
						{
							if (string.IsNullOrEmpty(this.ObjectName))
								return "Невозможно создать связь. Объект привязан у другому объекту.";
							else
								return string.Format("Невозможно создать связь. Объект '{0}' привязан у другому объекту.", this.ObjectName);
						}
						else
						{
							if (string.IsNullOrEmpty(this.ObjectName))
								return string.Format("Невозможно создать связь. Объект [{0}] привязан у другому объекту.", friendlyClassName);
							else
								return string.Format("Невозможно создать связь. Объект '{0}' [{1}] был удален другим пользователем.", this.ObjectName, friendlyClassName);
						}
				}
				//
				return base.Message;
			}
		}
		#endregion


		#region constructors
		public ObjectConstraintException(Type objectType, Guid objectID, ObjectConstraintReason reason)
			: this(objectType, objectID, null, reason)
		{ }

		public ObjectConstraintException(Guid objectID, string objectName, ObjectConstraintReason reason)
			: this(null, objectID, objectName, reason)
		{ }

		public ObjectConstraintException(Type objectType, Guid objectID, string objectName, ObjectConstraintReason reason)
			: base()
		{
			this.ObjectType = objectType;
			this.ObjectID = objectID;
			this.ObjectName = objectName;
			this.Reason = reason;
		}

		public ObjectConstraintException()
			: base("Нарушение ограничений целостности.")
		{ }

		public ObjectConstraintException(string message)
			: base(message)
		{ }

		public ObjectConstraintException(string message, Exception innerException)
			: base(message, innerException)
		{ }

		private ObjectConstraintException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
		#endregion
	}
}

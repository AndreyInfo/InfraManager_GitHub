using System;
using InfraManager.Core.Exceptions;

namespace InfraManager.Core
{
	//
	//TODO: hueta
	//
	public static class Validator
	{
		#region method ValidateName
		public static void ValidateName(string value, int maxLength)
		{
			ValidateField("Название", value, maxLength, false);
		}
		#endregion

		#region method ValidateNote
		public static void ValidateNote(string value, int maxLength)
		{
			ValidateField("Примечание", value, maxLength, true);
		}
		#endregion

		#region method ValidateField
		public static void ValidateField(string fieldName, string fieldValue, int maxLength)
		{
			ValidateField(fieldName, fieldValue, maxLength, true);
		}

		public static void ValidateField(string fieldName, string fieldValue, int maxLength, bool emptyValueIsAllowed)
		{
			if (fieldName == null)
				throw new ArgumentNullException("fieldName");
			if (fieldValue == null)
				throw new ArgumentNullException("fieldValue");
			if (fieldName.Length > 0 && char.IsLower(fieldName[0]))
				fieldName = string.Concat(char.ToUpper(fieldName[0]), fieldName.Substring(1));
			if (!emptyValueIsAllowed && fieldValue.Length == 0)
				throw new ArgumentValidationException(string.Format("Поле \"{0}\" должно быть задано.",
					fieldName));
			if (fieldValue.Length > maxLength)
				throw new ArgumentValidationException(fieldName, string.Format("Длина поля \"{0}\" не должна превышать {1} символов.",
					fieldName,
					maxLength));
		}
		#endregion
	}
}

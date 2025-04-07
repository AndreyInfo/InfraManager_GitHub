
namespace InfraManager.Core
{
	//
	//TODO: hueta
	//
	public static class Messages
	{
		public const string AddModeFormAcceptButtonText = "Добавить";
		public const string PropertiesModeFormAcceptButtonText = "Сохранить";
		public const string FieldChanged = "Поле изменено";


		#region method GetAddModeFormTitle
		public static string GetAddModeFormTitle(string friendlyClassName)
		{
			return string.Concat(friendlyClassName, " / Добавление");
		}
		#endregion

		#region method GetPropertiesModeFormTitle
		public static string GetPropertiesModeFormTitle(string friendlyClassName)
		{
			return string.Concat(friendlyClassName, " / Свойства");
		}
		#endregion
	}
}

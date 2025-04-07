using System.IO;
using System.Reflection;

namespace InfraManager.Core.Helpers
{
	public static class AssemblyHelper
	{
		#region method GetResourceText
		public static string GetResourceText(string resourceName)
		{
			using (var streamReader = new StreamReader(Assembly.GetCallingAssembly().GetManifestResourceStream(resourceName)))
				return streamReader.ReadToEnd();
		}

		public static string GetResourceText(Assembly assembly, string resourceName)
		{
			using (var streamReader = new StreamReader(assembly.GetManifestResourceStream(resourceName)))
				return streamReader.ReadToEnd();
		}
		#endregion
	}
}

using System;
using System.Reflection;

namespace InfraManager.ComponentModel
{
	public abstract class Singleton<T> where T : class
	{
		#region fields
		private static readonly T __instance = (T)typeof(T).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod)[0].Invoke(new object[0]); //required for singleton pattern
		#endregion


		#region properties
		public static T Instance { get { return __instance; } } //required for singleton pattern
		#endregion


		#region constructors
		static Singleton() { }

		protected Singleton() { }
		#endregion
	}
}

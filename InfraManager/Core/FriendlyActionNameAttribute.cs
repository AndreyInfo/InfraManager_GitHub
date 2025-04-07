using System;

namespace InfraManager.Core
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class FriendlyActionNameAttribute : Attribute
	{
		#region properties
		public string AddActionName { get; private set;}
		
		public string SaveActionName { get; private set;}
		
		public string RemoveActionName { get; private set;}
		#endregion


		#region constructors
		public FriendlyActionNameAttribute(string addActionName, string saveActionName, string removeActionName)
		{
			this.AddActionName = addActionName;
			this.SaveActionName = saveActionName;
			this.RemoveActionName = removeActionName;
		} 
		#endregion
	}
}

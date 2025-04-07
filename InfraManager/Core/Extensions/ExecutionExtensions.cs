using System;
using System.Threading;

namespace InfraManager.Core.Extensions
{
	public static class ExecutionExtensions
	{
		#region Execute
		public static bool Execute(this Action action)
		{
			try
			{
				action();
				return true;
			}
			catch { }
			return false;
		}

		public static bool Execute(this Action action, int attempts)
		{
			for (int i = 0; i < attempts; i++ )
				try
				{
					action();
					return true;
				}
				catch { }
			return false;
		}

		public static bool Execute(this Action action, int attempts, int millisecondsTimeout)
		{
			for (int i = 0; i < attempts; i++)
				try
				{
					action();
					return true;
				}
				catch 
				{
					Thread.Sleep(millisecondsTimeout);
				}
			return false;
		}
		#endregion
	}
}

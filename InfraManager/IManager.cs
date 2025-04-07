using System;

namespace InfraManager.ComponentModel
{
	public interface IManager
	{
		bool IsInitialized { get; }

		bool IsRunning { get; }


		void Initialize(params object[] arguments);

		void Terminate();

		void Start();

		void Stop();
	}
}

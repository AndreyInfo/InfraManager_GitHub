using System;

namespace InfraManager.ComponentModel
{
	public interface IModificationTracker
	{
		bool IsModified { get; }

		void MarkAsModified();

		void MarkAsNonModified();
	}
}

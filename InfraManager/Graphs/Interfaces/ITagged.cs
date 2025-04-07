using System;

namespace InfraManager.DataStructures.Graphs.Interfaces
{
	public interface ITagged<TTag>
	{
		event EventHandler TagChanged;

		TTag Tag { get; set; }
	}
}

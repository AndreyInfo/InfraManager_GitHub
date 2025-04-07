using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace InfraManager.ComponentModel
{
	public interface IComposite
	{
		bool CanBeParentedTo(IComposite parent);

		void GetChild(int index);

		void AddChild(IComposite child);

		void RemoveChild(IComposite child);


		IComparable Parent { get; set; }

		int ChildrenCount { get; }

		IEnumerable<IComposite> Children { get; }

		bool IsRoot { get; }

		bool IsLeaf { get; }
	}
}

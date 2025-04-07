using System;

namespace InfraManager.ComponentModel
{
	public interface ISummaryProvider
	{
		object[] GetUniqueVector();

		//
		//TODO: FW 4.0
		//
		//Tuple<string, obejct>[] GetNamedParameters();

		string ToString();
	}
}

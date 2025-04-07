
namespace InfraManager.ComponentModel
{
	public interface IActivatable
	{
		bool IsActivated { get; }

		bool IsSuccessful { get; }

		void Activate();
	}
}

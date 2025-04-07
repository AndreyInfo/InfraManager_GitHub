namespace InfraManager.DAL
{

    public enum TaskStateEnum : int
	{
		
		Inactive = 0,
		
		Waiting = 1,
		
		Running = 2,
		
		Finished = 3,
		
		StartWithError = 4,
		
		Error = 5

	}
}

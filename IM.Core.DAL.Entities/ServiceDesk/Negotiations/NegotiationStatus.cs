namespace InfraManager.DAL.ServiceDesk.Negotiations
{
    public enum NegotiationStatus : byte
    {
        /// <summary>
        /// Не начато
        /// </summary>
        Created = 0,

        /// <summary>
        /// Ожидает согласования
        /// </summary>
        Voting = 1,

        /// <summary>
        /// Одобрено
        /// </summary>
        For = 2,

        /// <summary>
        /// Отклонено
        /// </summary>
        Against = 3
    }
}

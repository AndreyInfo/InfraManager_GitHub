namespace InfraManager.DAL
{
    public interface IMarkableForDelete
    {
        // TODO: Хорошее решение, но должен быть Setter, это даст больше гибкости.
        bool Removed { get; }

        // TODO: Этим мы только что создали бойлерплейт код в сущностях.
        void MarkForDelete();
    }
}

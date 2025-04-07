
namespace InfraManager.DAL
{
    /// <summary>
    /// Этот класс представляет абстрактную сущность Упорядоченный справочник
    /// </summary>
    public abstract class SequencedLookup : Lookup
    {
        public SequencedLookup() : base()
        {
        }

        public SequencedLookup(string name, int sequence) : base(name)
        {
            Sequence = sequence;
        }

        /// <summary>
        /// Возвращает или задает порядок
        /// </summary>
        public int Sequence { get; set; }
    }
}

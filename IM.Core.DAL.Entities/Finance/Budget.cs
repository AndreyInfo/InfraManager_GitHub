using System;

namespace InfraManager.DAL.Finance
{
    public class Budget : IMarkableForDelete
    {
        public Guid ID { get; init; }
        public string Name { get; set; }
        public virtual Budget Parent { get; set; }
        public bool Removed { get; private set; }
        public byte[] RowVersion { get; set; }
        public string Code { get; set; }
        public string ExternalID { get; set; }

        public void MarkForDelete()
        {
            Removed = true;
        }

        public static string GetFullBudgetName(Guid id) => throw new NotSupportedException();
    }
}

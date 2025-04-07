using System.Collections.Generic;

namespace InfraManager.DAL.Accounts
{
    public class Tag
    {
        protected Tag()
        {
        }

        public Tag(string name)
        {
            Name = name;
        }

        public Tag(int id, string name)
        {
            Name = name;
            ID = id;
        }

        public int ID { get; init; }

        /// <summary>
        /// Имя тега
        /// </summary>
        public string Name { get; init; }

        public virtual ICollection<UserAccountTag> UserAccountTag { get; init; }
    }
}

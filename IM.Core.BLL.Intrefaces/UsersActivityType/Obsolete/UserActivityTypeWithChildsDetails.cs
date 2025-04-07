using InfraManager.DAL;
using System;
using System.Collections.Generic;

namespace InfraManager.BLL.UsersActivityType.Obsolete
{
    public class UserActivityTypeWithChildsDetails
    {
        public UserActivityTypeWithChildsDetails()
        {
        }
        public Guid Id { get; init; }
        public string Path { get; set; }
        public IEnumerable<UserActivityType> Types { get; private set; }
        public virtual UserActivityType Type { get; init; }

        public void BuildParents()
        {
            var type = Type;
            var result = new Queue<UserActivityType>();
            result.Enqueue(type);
            while (type.ParentID.HasValue)
            {
                type = type.Parent
                    ?? throw new ArgumentNullException("Должен быть инициализрованный родитель");
                result.Enqueue(type);
            }

            Types = result;
        }
    }
}

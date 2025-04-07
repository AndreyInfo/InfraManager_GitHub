using InfraManager.DAL.Accounts;
using System.Collections.Generic;

namespace InfraManager.BLL.Accounts.Tags
{
    public class TagDetailsModel
    {
        public int UserAccountID { get; set; }
        public List<Tag> Tags { get; set; }
    }
}

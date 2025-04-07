using System;

namespace InfraManager.BLL.Users
{
    public class UserListItem
    {
        [Obsolete("Use IMObjID instead")]
        public Guid ID { get; init; } //TODO: В админке использовать IMObjID вместо ID, а в идеале UserDetails должен либо совпадать с ListItem, либо расширять его
        public Guid IMObjID { get; init; }
        public string Name { get; init; }
        public Guid? SubdivisionID { get; init; }
        public string Details { get; set; }
    }
}

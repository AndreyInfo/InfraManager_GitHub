using InfraManager.DAL.Message;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.Messages
{
    internal class MessageByEMailQuery : IMessageByEMailQuery, ISelfRegisteredService<IMessageByEMailQuery>
    {
        private readonly DbContext _db;

        public MessageByEMailQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public IQueryable<MessageByEmail> Query(Guid userId, Expression<Func<MessageByEmail, bool>> filterBy)
        {
            IQueryable<MessageByEmail> messages = _db.Set<MessageByEmail>().AsNoTracking();

            if (filterBy != null)
                messages = messages.Where(filterBy);
            return messages;
        }
    }
}

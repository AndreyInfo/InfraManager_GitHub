using InfraManager.DAL.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.Messages
{
    public interface IMessageByEMailQuery
    {
        IQueryable<MessageByEmail> Query(Guid userId, Expression<Func<MessageByEmail, bool>> filterBy);
    }
}

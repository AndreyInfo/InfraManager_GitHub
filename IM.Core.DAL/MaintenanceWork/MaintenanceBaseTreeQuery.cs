using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace InfraManager.DAL.MaintenanceWork;

internal abstract class MaintenanceBaseTreeQuery<TEntity>
    where TEntity : class
{
    protected readonly DbSet<ClassIcon> ClassIcons;
    protected readonly DbSet<TEntity> Entities;

    protected abstract ObjectClass ClassID { get; }

    protected Expression<Func<ClassIcon, bool>> IconNameExpression => c => c.ClassID == ClassID;

    public MaintenanceBaseTreeQuery(DbSet<ClassIcon> classIcons
        , DbSet<TEntity> entities)
    {
        ClassIcons = classIcons;
        Entities = entities;
    }
}
using AutoMapper;
using Inframanager;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.Linq;
using System;
using System.Linq.Expressions;

namespace InfraManager.DAL.ServiceDesk
{
    internal static class AutoMapperExtensions
    {
        #region Common

        public static IMappingExpression<TSource, TDest> With<TSource, TDest, TProp>(
            this IMappingExpression<TSource, TDest> mapping,
            Expression<Func<TDest, TProp>> destinationProperty,
            Expression<Func<TSource, TProp>> sourceProperty) =>
            mapping.ForMember(destinationProperty, mapper => mapper.MapFrom(sourceProperty));

        public static IMappingExpression<TSource, TDest> WithNull<TSource, TDest, TProp>(
            this IMappingExpression<TSource, TDest> mapping,
            Expression<Func<TDest, TProp>> destinationProperty) where TProp : class =>
            mapping.With(destinationProperty, x => null);

        public static IMappingExpression<TSource, TDest> WithNull<TSource, TDest, TProp>(
            this IMappingExpression<TSource, TDest> mapping,
            Expression<Func<TDest, Nullable<TProp>>> destinationProperty) where TProp : struct =>
            mapping.With(destinationProperty, x => null);

        public static IMappingExpression<TSource, TDest> WithCast<TSource, TDest>(
            this IMappingExpression<TSource, TDest> mapping,
            Expression<Func<TDest, string>> destinationProperty,
            Expression<Func<TSource, string>> sourceProperty)
        {
            Expression<Func<string, string>> castExpression = s => DbFunctions.CastAsString(s);

            return mapping.With(destinationProperty, castExpression.Substitute(sourceProperty));
        }

        public static IMappingExpression<TEntity, TQueryResultItem> WithCategory<TEntity, TQueryResultItem>(
            this IMappingExpression<TEntity, TQueryResultItem> mapping,
            Issues category)
            where TQueryResultItem : IssueQueryResultItem
        {
            Expression<Func<Group, string>> groupName = x => DbFunctions.CastAsString(x.Name);

            return mapping.With(
                item => item.CategorySortColumn,
                x => category);
        }

        public static IMappingExpression<TEntity, TQueryResultItem> WithGroupName<TEntity, TQueryResultItem>(
            this IMappingExpression<TEntity, TQueryResultItem> mapping,
            Expression<Func<TEntity, Group>> groupExpression) where TQueryResultItem : IssueQueryResultItem
        {
            Expression<Func<Group, string>> groupName = x => DbFunctions.CastAsString(x.Name);

            return mapping.With(item => item.QueueName, groupName.Substitute(groupExpression));
        }

        public static IMappingExpression<TEntity, TQueryResultItem> WithClient<TEntity, TQueryResultItem>(
            this IMappingExpression<TEntity, TQueryResultItem> mapping,
            Expression<Func<TEntity, User>> clientAccessor) where TQueryResultItem : IssueQueryResultItem
        {
            Expression<Func<User, Guid?>> userID = u => u.IMObjID;

            return mapping
                .With(x => x.ClientID, userID.Substitute(clientAccessor))
                .WithCast(x => x.ClientFullName, User.FullNameExpression.Substitute(clientAccessor));
        }

        public static IMappingExpression<TEntity, TQueryResultItem> WithClientCommon<TEntity, TQueryResultItem>(
            this IMappingExpression<TEntity, TQueryResultItem> mapping,
            Expression<Func<TEntity, User>> clientAccessor) where TQueryResultItem : IssueQueryResultItem
        {
            Expression<Func<User, Guid?>> userID = u => u.IMObjID;
            Expression<Func<User, string>> subdivisionFullName = u => Subdivision.GetFullSubdivisionName(u.SubdivisionID);
            Expression<Func<User, string>> organizationName = u => u.Subdivision.Organization.Name;

            return mapping
                .WithClient(clientAccessor)
                .With(x => x.ClientSubdivisionFullName, subdivisionFullName.Substitute(clientAccessor))
                .WithCast(x => x.ClientOrganizationName, organizationName.Substitute(clientAccessor));
        }

        #endregion

        #region Under Control report query

        public static IMappingExpression<T, ObjectUnderControlQueryResultItem> WithClientUnderControl<T>(
            this IMappingExpression<T, ObjectUnderControlQueryResultItem> mapping,
            Expression<Func<T, User>> clientAccessor)
        {
            Expression<Func<User, Guid?>> subdivisionID = u => u.SubdivisionID;
            Expression<Func<User, Guid?>> organizationID = u => u.Subdivision.OrganizationID;

            return mapping
                .With(x => x.ClientSubdivisionID, subdivisionID.Substitute(clientAccessor))
                .With(x => x.ClientOrganizationID, organizationID.Substitute(clientAccessor));
        }

        public static IMappingExpression<TEntity, ObjectUnderControlQueryResultItem> WithNoteAndMessageCounts<TEntity>(
            this IMappingExpression<TEntity, ObjectUnderControlQueryResultItem> mapping,
            NoteCountExpressionCreator<TEntity> noteCountCreator)
            where TEntity : IGloballyIdentifiedEntity
        {
            return mapping
                .ForMember(item => item.NoteCount, mapper => mapper.MapFrom(noteCountCreator.Create(SDNoteType.Note)))
                .ForMember(item => item.MessageCount, mapper => mapper.MapFrom(noteCountCreator.Create(SDNoteType.Message)));
        }

        #endregion
    }
}

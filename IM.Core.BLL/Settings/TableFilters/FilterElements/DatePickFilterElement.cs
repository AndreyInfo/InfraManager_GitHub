using InfraManager.BLL.Settings.TableFilters.FilterElements.ExpressionBuilders;
using InfraManager.DAL.Settings;
using InfraManager.Expressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfraManager.BLL.Settings.TableFilters.FilterElements
{
    internal class DatePickFilterElement : FilterElementBase
    {
        #region .ctor

        public DatePickFilterElement(FilterElementData elementData) : base(elementData)
        {
            StartDate = DateTimeExtensions.TryConvertFromMillisecondsAfterMinimumDate(
                elementData.StartDate, 
                out DateTime? startDate)
                ? startDate
                : throw new ArgumentException(
                    $"Invalid {nameof(elementData.StartDate)} value", 
                    nameof(elementData));
            FinishDate = DateTimeExtensions.TryConvertFromMillisecondsAfterMinimumDate(
                elementData.FinishDate, 
                out DateTime? finishDate)
                ? finishDate
                : throw new ArgumentException(
                    $"Invalid {nameof(elementData.FinishDate)} value",
                    nameof(elementData));
            Operation = (DateTimeSearchOperation?)elementData.SearchOperation;
            OnlyDate = elementData.OnlyDate;
        }

        public DatePickFilterElement(FilterElementDetails model) : base(model)
        {
            StartDate = DateTimeExtensions.TryConvertFromMillisecondsAfterMinimumDate(
                model.StartDate, 
                out DateTime? start)
                ? start
                : throw new InvalidObjectException($"Недопустимое значение миллисекунд {model.StartDate}");
            FinishDate = DateTimeExtensions.TryConvertFromMillisecondsAfterMinimumDate(
                model.FinishDate,
                out DateTime? finish)
                ? finish
                : throw new InvalidObjectException($"Недопустимое значение миллисекунд {model.FinishDate}");
            Operation = (DateTimeSearchOperation?)model.Operation;
            OnlyDate = model.OnlyDate;
        }

        #endregion

        #region Properties

        public DateTime? StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public DateTimeSearchOperation? Operation { get; set; }

        public bool OnlyDate { get; set; }

        #endregion

        #region Build FilterElement Data

        protected override void AssignDataAttributes(FilterElementData data)
        {
            data.StartDate = StartDate.ConvertToMillisecondsAfterMinimumDate();
            data.FinishDate = FinishDate.ConvertToMillisecondsAfterMinimumDate();
            data.OnlyDate = OnlyDate;
            data.SearchOperation = (byte?)Operation;
        }

        #endregion

        #region Build Predicate expression

        private static Dictionary<DateTimeSearchOperation, IDateTimePredicateBuilder> _predicateBuilders =
            new Dictionary<DateTimeSearchOperation, IDateTimePredicateBuilder>
            {
                { DateTimeSearchOperation.After, new DateTimeAfterExpressionBuilder() },
                { DateTimeSearchOperation.Before, new DateTimeBeforeExpressionBuilder() },
                { DateTimeSearchOperation.Equal, new DateTimeEqualExpressionBuilder() },
                { DateTimeSearchOperation.Except, new DateTimeExceptExpressionBuilder() },
                { 
                    DateTimeSearchOperation.LastHour, 
                    new DateTimeUtcNowBasedExpressionBuilder(
                        now => now.TruncateMinutes(), 
                        now => now)
                },
                {
                    DateTimeSearchOperation.LastMonth,
                    new DateTimeUtcNowBasedExpressionBuilder(
                        now => now.BeginningOfMonth(), 
                        now => DateTime.UtcNow)
                },
                {
                    DateTimeSearchOperation.LastQuarter,
                    new DateTimeUtcNowBasedExpressionBuilder(
                        now => now.BeginningOfQuarter(),
                        now => now)
                },
                {
                    DateTimeSearchOperation.LastWeek,
                    new DateTimeUtcNowBasedExpressionBuilder(
                        now => now.BeginningOfWeek(),
                        now => now)
                },
                {
                    DateTimeSearchOperation.LastYear,
                    new DateTimeUtcNowBasedExpressionBuilder(
                        now => new DateTime(now.Year, 1, 1),
                        now => now)
                },
                {
                    DateTimeSearchOperation.NextHalfYear,
                    new DateTimeUtcNowBasedExpressionBuilder(
                        now => now.BeginningOfHalfYear().AddHalfYear(),
                        now => now.AddHalfYear().AddHalfYear().BeginningOfHalfYear())
                },
                {
                    DateTimeSearchOperation.NextMonth,
                    new DateTimeUtcNowBasedExpressionBuilder(
                        now => now.BeginningOfMonth().AddMonths(1),
                        now => now.AddMonths(2).BeginningOfMonth())
                },
                {
                    DateTimeSearchOperation.NextQuarter,
                    new DateTimeUtcNowBasedExpressionBuilder(
                        now => now.AddQuarter(1).BeginningOfQuarter(),
                        now => now.AddQuarter(2).BeginningOfQuarter())
                },
                {
                    DateTimeSearchOperation.NextYear,
                    new DateTimeUtcNowBasedExpressionBuilder(
                        now => new DateTime(now.Year + 1, 1, 1),
                        now => new DateTime(now.Year + 2, 1, 1))
                },
                {
                    DateTimeSearchOperation.PastHour,
                    new DateTimeUtcNowBasedExpressionBuilder(
                        now => now.AddHours(-1).TruncateMinutes(),
                        now => now.TruncateMinutes())
                },
                {
                    DateTimeSearchOperation.PastMonth,
                    new DateTimeUtcNowBasedExpressionBuilder(
                        now => now.AddMonths(-1).BeginningOfMonth(),
                        now => now.BeginningOfMonth())
                },
                {
                    DateTimeSearchOperation.PastQuarter,
                    new DateTimeUtcNowBasedExpressionBuilder(
                        now => now.AddQuarter(-1).BeginningOfQuarter(),
                        now => now.BeginningOfQuarter())
                },
                {
                    DateTimeSearchOperation.PastWeek,
                    new DateTimeUtcNowBasedExpressionBuilder(
                        now => now.AddDays(-7).BeginningOfWeek(),
                        now => now.BeginningOfWeek())
                },
                {
                    DateTimeSearchOperation.PastYear,
                    new DateTimeUtcNowBasedExpressionBuilder(
                        now => new DateTime(now.Year - 1, 1, 1),
                        now => new DateTime(now.Year, 1, 1))
                },
                {
                    DateTimeSearchOperation.PresentHour,
                    new DateTimeUtcNowBasedExpressionBuilder(
                        now => now.TruncateMinutes(),
                        now => now)
                },
                {
                    DateTimeSearchOperation.PresentMonth,
                    new DateTimeUtcNowBasedExpressionBuilder(
                        now => now.BeginningOfMonth(),
                        now => now)
                },
                {
                    DateTimeSearchOperation.PresentQuarter,
                    new DateTimeUtcNowBasedExpressionBuilder(
                        now => now.BeginningOfQuarter(),
                        now => now)
                },
                {
                    DateTimeSearchOperation.PresentWeek,
                    new DateTimeUtcNowBasedExpressionBuilder(
                        now => now.BeginningOfWeek(),
                        now => now)
                },
                {
                    DateTimeSearchOperation.PresentYear,
                    new DateTimeUtcNowBasedExpressionBuilder(
                        now => new DateTime(now.Year, 1, 1),
                        now => now)
                },
                {
                    DateTimeSearchOperation.Today,
                    new DateTimeUtcNowBasedExpressionBuilder(
                        now => now.Date,
                        now => now)
                },
                {
                    DateTimeSearchOperation.Yesterday,
                    new DateTimeUtcNowBasedExpressionBuilder(
                        now => now.AddDays(-1).Date,
                        now => now.Date)
                },
            };

        protected override Expression<Func<TProperty, bool>> OperationPredicate<TProperty>(
            IEnumerable<IBuildPredicateParameter> parameterBuilders)
        {
            if (typeof(TProperty) != typeof(DateTime) && typeof(TProperty) != typeof(DateTime?))
            {
                throw new NotSupportedException($"DatePick filter does not support {typeof(TProperty)} data type");
            }                        
            
            var predicateBuilder = Operation.HasValue && _predicateBuilders.TryGetValue(Operation.Value, out var builder)
                ? builder
                : new DateTimeBetweenExpressionBuilder();

            LambdaExpression expression = typeof(TProperty) == typeof(DateTime) 
                ?  predicateBuilder.Build(StartDate, FinishDate, OnlyDate)
                :  predicateBuilder.BuildNullable(StartDate, FinishDate, OnlyDate);

            return Expression.Lambda<Func<TProperty, bool>>(expression.Body, expression.Parameters[0]);
        }

        #endregion
    }
}

using System;
using System.Diagnostics.CodeAnalysis;
using HotChocolate.Language;
using HotChocolate.Types.Filters.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace HotChocolate.Types.Filters.Mongo
{
    public static partial class StringOperationHandlers
    {
        public static bool In(
            FilterOperation operation,
            IInputType type,
            IValueNode value,
            IFilterVisitorContext<IMongoQuery> context,
            [NotNullWhen(true)] out IMongoQuery? result)
        {
            object parsedValue = type.ParseLiteral(value);

            if (parsedValue == null)
            {
                context.ReportError(
                    ErrorHelper.CreateNonNullError(operation, type, value, context));

                result = null;
                return false;
            }

            if (operation.Type == typeof(string) &&
                type.IsInstanceOfType(value) &&
                context is MongoFilterVisitorContext ctx)
            {
                result = Query.In(
                    ctx.GetMongoFilterScope().GetPath(), BsonArray.Create(parsedValue));
                return true;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public static bool NotIn(
            FilterOperation operation,
            IInputType type,
            IValueNode value,
            IFilterVisitorContext<IMongoQuery> context,
            [NotNullWhen(true)] out IMongoQuery? result)
        {
            object parsedValue = type.ParseLiteral(value);

            if (parsedValue == null)
            {
                context.ReportError(
                    ErrorHelper.CreateNonNullError(operation, type, value, context));

                result = null;
                return false;
            }

            if (operation.Type == typeof(IComparable) &&
                type.IsInstanceOfType(value) &&
                context is MongoFilterVisitorContext ctx)
            {
                result = Query.Not(Query.In(
                    ctx.GetMongoFilterScope().GetPath(), BsonArray.Create(parsedValue)));

                return true;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
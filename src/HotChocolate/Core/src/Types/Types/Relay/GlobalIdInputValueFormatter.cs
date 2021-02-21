using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using HotChocolate.Internal;

#nullable enable

namespace HotChocolate.Types.Relay
{
    internal class GlobalIdInputValueFormatter : IInputValueFormatter
    {
        private readonly NameString _schemaName;
        private readonly NameString _typeName;
        private readonly IIdSerializer _idSerializer;
        private readonly bool _validateType;
        private readonly Func<IList> _createList;
        private readonly Type _valueType;
        private readonly DeserializeId _deserializeIdFunction;

        public GlobalIdInputValueFormatter(
            NameString schemaName,
            NameString typeName,
            IIdSerializer idSerializer,
            IExtendedType resultType,
            bool validateType,
            DeserializeId deserializeIdFunction)
        {
            _schemaName = schemaName;
            _typeName = typeName;
            _idSerializer = idSerializer;
            _validateType = validateType;
            _createList = CreateListFactory(resultType);
            _valueType = resultType.Source;
            _deserializeIdFunction = deserializeIdFunction;
        }

        public object? OnAfterDeserialize(object? runtimeValue)
        {
            if (runtimeValue is null)
            {
                return null;
            }

            if (runtimeValue is string s)
            {
                try
                {
                    IdValue id = DeserializeId(s);

                    if (!_validateType || _typeName.Equals(id.TypeName))
                    {
                        return id.Value;
                    }
                }
                catch
                {
                    throw new GraphQLException(
                        ErrorBuilder.New()
                            .SetMessage("The ID `{0}` has an invalid format.", s)
                            .Build());
                }

                throw new GraphQLException(
                    ErrorBuilder.New()
                        .SetMessage("The ID `{0}` is not an ID of `{1}`.", s, _typeName)
                        .Build());
            }

            if (runtimeValue is IEnumerable<string> stringEnumerable)
            {
                try
                {
                    IList list = _createList();

                    foreach (string sv in stringEnumerable)
                    {
                        IdValue id = DeserializeId(sv);

                        if (!_validateType || _typeName.Equals(id.TypeName))
                        {
                            list.Add(id.Value);
                        }
                    }

                    return list;
                }
                catch
                {
                    throw new GraphQLException(
                        ErrorBuilder.New()
                            .SetMessage(
                                "The IDs `{0}` have an invalid format.",
                                string.Join(", ", stringEnumerable))
                            .Build());
                }
            }

            throw new GraphQLException(
                ErrorBuilder.New()
                    .SetMessage("The specified value is not a valid ID value.")
                    .Build());
        }

        private IdValue DeserializeId(string serializedId) =>
            _deserializeIdFunction(_idSerializer, _schemaName, _typeName, _valueType, serializedId);

        private static Func<IList> CreateListFactory(IExtendedType resultType)
        {
            if (resultType.IsArrayOrList)
            {
                Type listType = typeof(List<>).MakeGenericType(resultType.ElementType!.Source);
                ConstructorInfo constructor = listType.GetConstructors().Single(t => t.GetParameters().Length == 0);
                Expression create = Expression.New(constructor);
                return Expression.Lambda<Func<IList>>(create).Compile();
            }

            return () => throw new NotSupportedException("Lists are not supported!");
        }
    }
}

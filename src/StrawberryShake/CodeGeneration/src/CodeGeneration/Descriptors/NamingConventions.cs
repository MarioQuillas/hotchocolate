namespace StrawberryShake.CodeGeneration
{
    public static class NamingConventions
    {
        public static string ResultInfoNameFromTypeName(
            string typeName) =>
            typeName + "Info";

        public static string MutationServiceNameFromTypeName(
            string typeName) =>
            typeName + "Mutation";

        public static string SubscriptionServiceNameFromTypeName(
            string typeName) =>
            typeName + "Subscription";

        public static string QueryServiceNameFromTypeName(
            string typeName) =>
            typeName + "Query";

        public static string EntityTypeNameFromGraphQLTypeName(
            string typeName) =>
            typeName + "Entity";

        public static string DocumentTypeNameFromOperationName(
            string typeName) =>
            typeName + "Document";

        public static string DataMapperNameFromGraphQLTypeName(
            string typeName, 
            string graphqlTypename) =>
            typeName + "From" + DataTypeNameFromTypeName(graphqlTypename) + "Mapper";

        public static string EntityMapperNameFromGraphQLTypeName(
            string typeName, 
            string graphqlTypename) =>
            typeName + "From" + EntityTypeNameFromGraphQLTypeName(graphqlTypename) + "Mapper";

        public static string RequestNameFromOperationServiceName(
            string operationServiceName) =>
            operationServiceName + "Request";

        public static string ResultFactoryNameFromTypeName(string typeName) =>
            typeName + "Factory";

        public static string ResultBuilderNameFromTypeName(string typeName) =>
            typeName + "Builder";

        public static string DataTypeNameFromTypeName(string typeName) =>
            typeName + "Data";
    }
}
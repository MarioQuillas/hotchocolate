using System;

namespace HotChocolate.Data.Neo4J
{
    /// <summary>
    /// Represents methods that can be used to ensure that parameter values meet expected conditions.
    /// </summary>
    public static class Ensure
    {
        /// <summary>
        /// Ensures that the value of a parameter is not null.
        /// </summary>
        /// <typeparam name="T">Type type of the value.</typeparam>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns>The value of the parameter.</returns>
        public static T IsNotNull<T>(T value, string paramName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName, "Value cannot be null.");
            }
            return value;
        }
    }
}
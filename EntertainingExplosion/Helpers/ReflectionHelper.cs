using System;
using System.Linq.Expressions;

namespace EntertainingExplosion.Helpers
{
    public static class ReflectionHelper
    {
        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>Name of property</returns>
        public static String GetPropertyName<T>(this Expression<Func<T>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            return memberExpression == null
                ? String.Empty
                : memberExpression.Member.Name;
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>Name of property</returns>
        public static String GetPropertyName<T>(this Expression<Func<T, Object>> expression)
        {
            var body = expression.Body as UnaryExpression;
            var memberExpression = body != null ?
                (MemberExpression)body.Operand :
                (MemberExpression)expression.Body;
            return memberExpression.Member.Name;
        }

        public static string GetMemberName<TArg, TRes>(this Expression<Func<TArg, TRes>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                var unaryExpression = propertyExpression.Body as UnaryExpression;
                if (unaryExpression != null)
                    memberExpression = unaryExpression.Operand as MemberExpression;
            }
            if (memberExpression != null)
            {
                var parameterExpression = memberExpression.Expression as ParameterExpression;
                if (parameterExpression != null && parameterExpression.Name == propertyExpression.Parameters[0].Name)
                    return memberExpression.Member.Name;
            }
            throw new ArgumentException("Invalid expression.", "propertyExpression");
        }
    }
}

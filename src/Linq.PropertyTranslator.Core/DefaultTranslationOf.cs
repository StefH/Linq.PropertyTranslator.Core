using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Linq.PropertyTranslator.Core.Validation;

namespace Linq.PropertyTranslator.Core
{
    /// <summary>
    /// Registers property mappings to the default <see cref="TranslationMap"/> and provides access to them.
    /// </summary>
    /// <typeparam name="T">The object (e.g. entity) type.</typeparam>
    public static class DefaultTranslationOf<T>
    {
        /// <summary>
        /// Evaluates the registered expression for specified method and instance.
        /// </summary>
        /// <param name="instance">The object instance.</param>
        /// <param name="method">The method.</param>
        /// <returns>The result of the expression execution.</returns>
        /// <typeparam name="TResult">Type of the result of the expression.</typeparam>
        public static TResult Evaluate<TResult>([NotNull] T instance, [NotNull] MethodBase method)
        {
            Check.NotNull(instance, nameof(instance));
            Check.NotNull(method, nameof(method));

            return TranslationMap.DefaultMap.Get<T, TResult>(method).Evaluate(instance);
        }

        /// <summary>
        /// Property wrapper for specified object property.
        /// </summary>
        /// <param name="property">The property wrapper.</param>
        /// <returns></returns>
        /// <typeparam name="TResult">Type of the result of the expression.</typeparam>
        public static IncompletePropertyTranslation<TResult> Property<TResult>([NotNull] Expression<Func<T, TResult>> property)
        {
            Check.NotNull(property, nameof(property));

            return new IncompletePropertyTranslation<TResult>(property);
        }

        /// <summary>
        /// Registers a mapping for specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="language">The ui culture (e.g. "nl", "en", etc.).</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">On invalid property expression type (must be of type MemberExpression).</exception>
        /// <typeparam name="TResult">Type of the result of the expression.</typeparam>
        public static CompiledExpressionMap<T, TResult> Property<TResult>([NotNull] Expression<Func<T, TResult>> property, [NotNull] Expression<Func<T, TResult>> expression, [NotNull] string language = "")
        {
            Check.NotNull(property, nameof(property));
            Check.NotNull(expression, nameof(expression));
            Check.NotNull(language, nameof(language));

            return TranslationMap.DefaultMap.Add(property, expression, language);
        }

        /// <summary>
        /// Property wrapper for chained registration.
        /// </summary>
        /// <typeparam name="TResult">Type of the result of the expression.</typeparam>
        public class IncompletePropertyTranslation<TResult>
        {
            private readonly Expression<Func<T, TResult>> _property;

            /// <summary>
            /// Initializes a new instance of the <see cref="IncompletePropertyTranslation{TResult}" /> class.
            /// </summary>
            /// <param name="property">The property.</param>
            internal IncompletePropertyTranslation(Expression<Func<T, TResult>> property)
            {
                _property = property;
            }

            /// <summary>
            /// Registers the specified expression for current property and specified language.
            /// </summary>
            /// <param name="expression">The expression.</param>
            /// <param name="language">The language (optional).</param>
            /// <returns></returns>
            /// <exception cref="System.InvalidOperationException">On invalid property expression type (must be of type MemberExpression).</exception>
            public CompiledExpressionMap<T, TResult> Is(Expression<Func<T, TResult>> expression, string language = "")
            {
                try
                {
                    return Property(_property, expression, language);
                }
                catch (ArgumentException exception)
                {
                    throw new InvalidOperationException("Invalid expression type of property. Must be of type MemberExpression.", exception);
                }
            }
        }
    }
}
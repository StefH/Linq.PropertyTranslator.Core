using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Linq.PropertyTranslator.Core.Validation;
#if !(DNXCORE50 || NETSTANDARD)
using System.Runtime.Serialization;
#endif

namespace Linq.PropertyTranslator.Core
{
    /// <summary>
    /// Map for property translations.
    /// </summary>
#if !(DNXCORE50 || NETSTANDARD)
    [Serializable]
#endif
    public class TranslationMap : Dictionary<MemberInfo, CompiledExpressionMap>
    {
        /// <summary>
        /// Instance of the default <see cref="TranslationMap"/>.
        /// </summary>
        public static readonly TranslationMap DefaultMap = new TranslationMap();

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationMap" /> class.
        /// </summary>
        public TranslationMap()
        {
        }

#if !(DNXCORE50 || NETSTANDARD)
        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationMap" /> class.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        protected TranslationMap(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif

        /// <summary>
        /// Adds a new expression for specified property to the map.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="compiledExpression">The compiled expression.</param>
        /// <param name="language">The language (e.g. "de", "en", etc.).</param>
        /// <exception cref="System.ArgumentException">On invalid property expression type (must be of type MemberExpression).</exception>
        /// <typeparam name="T">The object (e.g. entity) type.</typeparam>
        /// <typeparam name="TResult">Type of the result of the expression.</typeparam>
        public void Add<T, TResult>([NotNull] Expression<Func<T, TResult>> property, [NotNull] CompiledExpression<T, TResult> compiledExpression, [NotNull] string language = "")
        {
            Check.NotNull(property, nameof(property));
            Check.NotNull(compiledExpression, nameof(compiledExpression));
            Check.NotNull(language, nameof(language));

            var member = property.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException("property body must be a MemberExpression.", nameof(property));

            AddInternal(member.Member, compiledExpression, language);
        }

        /// <summary>
        /// Adds a new expression for specified property to the map.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="language">The language (e.g. "de", "en", etc.).</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">On invalid property expression type (must be of type MemberExpression).</exception>
        /// <typeparam name="T">The object (e.g. entity) type.</typeparam>
        /// <typeparam name="TResult">Type of the result of the expression.</typeparam>
        public CompiledExpressionMap<T, TResult> Add<T, TResult>([NotNull] Expression<Func<T, TResult>> property, [NotNull] Expression<Func<T, TResult>> expression, [NotNull] string language = "")
        {
            Check.NotNull(property, nameof(property));
            Check.NotNull(expression, nameof(expression));
            Check.NotNull(language, nameof(language));

            var member = property.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException("property body must be a MemberExpression.", nameof(property));

            var compiledExpression = new CompiledExpression<T, TResult>(expression);

            return AddInternal(member.Member, compiledExpression, language);
        }

        /// <summary>
        /// Returns the <see cref="CompiledExpression" /> for specified method and the ui culture of the current thread.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns></returns>
        /// <typeparam name="T">The object (e.g. entity) type.</typeparam>
        /// <typeparam name="TResult">Type of the result of the expression.</typeparam>
        public CompiledExpression<T, TResult> Get<T, TResult>([NotNull] MethodBase method)
        {
            Check.NotNull(method, nameof(method));

            CompiledExpression result;

            if (TryGetValue(method, out result))
            {
                return result as CompiledExpression<T, TResult>;
            }

            throw new InvalidOperationException("No expression registered for specified method.");
        }

        /// <summary>
        /// Tries to return the <see cref="CompiledExpression" /> for specified method and the ui culture of the current thread.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="expression">The compiled expression.</param>
        /// <returns></returns>
        public bool TryGetValue([NotNull] MemberInfo method, out CompiledExpression expression)
        {
            Check.NotNull(method, nameof(method));
            Check.NotNull(method.DeclaringType, nameof(method.DeclaringType));

            return TryGetValue(method, method.DeclaringType, out expression);
        }

        /// <summary>
        /// Tries to return the <see cref="CompiledExpression" /> for specified method and the ui culture of the current thread.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="baseType">When the quey was built against an interface you can specify the concrete type here (retrieved from node.Expression.Type!).</param>
        /// <param name="expression">The compiled expression.</param>
        /// <returns></returns>
        public bool TryGetValue([NotNull] MemberInfo method, [NotNull] Type baseType, out CompiledExpression expression)
        {
            Check.NotNull(method, nameof(method));
            Check.NotNull(baseType, nameof(baseType));

            PropertyInfo property = baseType.GetProperty(method.Name.Replace("get_", string.Empty));

            if (property == null || !ContainsKey(property))
            {
                expression = null;

                return false;
            }

            var map = base[property];

            return map.TryGetValue(out expression);
        }

        private CompiledExpressionMap<T, TResult> AddInternal<T, TResult>(MemberInfo property, CompiledExpression<T, TResult> compiledExpression, string language)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                language = CompiledExpressionMap.DefaultLanguageKey;
            }

            if (!ContainsKey(property))
            {
                Add(property, new CompiledExpressionMap<T, TResult>());
            }

            base[property].Add(language.ToUpperInvariant(), compiledExpression);

            return base[property] as CompiledExpressionMap<T, TResult>;
        }
    }
}
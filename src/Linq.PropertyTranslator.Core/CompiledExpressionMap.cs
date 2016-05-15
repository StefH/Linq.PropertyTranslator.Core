using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
#if !(DNXCORE50 || NETSTANDARD || WINDOWS_APP)
using System.Runtime.Serialization;
#endif

namespace Linq.PropertyTranslator.Core
{
    /// <summary>
    /// Collection of <see cref="CompiledExpression"/>s for different UiCultures.
    /// </summary>
#if !(DNXCORE50 || NETSTANDARD || WINDOWS_APP)
    [Serializable]
#endif
    public class CompiledExpressionMap : Dictionary<string, CompiledExpression>
    {
        /// <summary>
        /// Default key for invariant language.
        /// </summary>
        public const string DefaultLanguageKey = "INVARIANT";

        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledExpressionMap" /> class.
        /// </summary>
        public CompiledExpressionMap()
        {
        }

#if !(DNXCORE50 || NETSTANDARD || WINDOWS_APP)
        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledExpressionMap" /> class.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        protected CompiledExpressionMap(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif

        /// <summary>
        /// Gets the compiled expression for current thread ui culture.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">If no <see cref="CompiledExpression"/> for current environment is available.</exception>
        public virtual CompiledExpression GetValue()
        {
            CompiledExpression result;

            if (TryGetValue(out result))
            {
                return result;
            }

            throw new InvalidOperationException("No expression registered for specified method.");
        }

        /// <summary>
        /// Tries to get the compiled expression for current thread ui culture.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public virtual bool TryGetValue(out CompiledExpression expression)
        {
            var language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToUpperInvariant();

            if (ContainsKey(language))
            {
                expression = this[language];
            }
            else if (ContainsKey(DefaultLanguageKey))
            {
                expression = this[DefaultLanguageKey];
            }
            else if (Count > 0)
            {
                expression = Values.First();
            }
            else
            {
                expression = null;
            }

            return expression != null;
        }
    }
}
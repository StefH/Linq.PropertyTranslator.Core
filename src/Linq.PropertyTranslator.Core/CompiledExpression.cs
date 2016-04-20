using System.Linq.Expressions;

namespace Linq.PropertyTranslator.Core
{
    /// <summary>
    /// Abstract, non-generic compiled expression.
    /// </summary>
    public abstract class CompiledExpression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledExpression" /> class.
        /// </summary>
        protected CompiledExpression()
        {
        }

        /// <summary>
        /// Gets the underlying lambda expression.
        /// </summary>
        /// <value>The lambda expression.</value>
        internal abstract LambdaExpression BaseExpression { get; }
    }
}
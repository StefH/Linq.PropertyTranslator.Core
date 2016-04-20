using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Linq.PropertyTranslator.Core.Validation;

namespace Linq.PropertyTranslator.Core
{
    /// <summary>
    /// Generic extension to the <see cref="CompiledExpression"/> class.
    /// </summary>
    /// <typeparam name="T">The object (e.g. entity) type.</typeparam>
    /// <typeparam name="TResult">Type of the result of the expression.</typeparam>
    public class CompiledExpression<T, TResult> : CompiledExpression
    {
        /// <summary>
        /// The base expression.
        /// </summary>
        private readonly Expression<Func<T, TResult>> _expression;

        /// <summary>
        /// The compiled expression.
        /// </summary>
        private readonly Func<T, TResult> _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledExpression{T, TResult}" /> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        public CompiledExpression([NotNull] Expression<Func<T, TResult>> expression)
        {
            Check.NotNull(expression, nameof(expression));

            _expression = expression;
            _function = expression.Compile();
        }

        /// <summary>
        /// Gets the undelying lambda expression.
        /// </summary>
        /// <value>The lambda expression.</value>
        internal override LambdaExpression BaseExpression => _expression;

        /// <summary>
        /// Evaluates the compiled expression on the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>TResult</returns>
        public TResult Evaluate(T instance)
        {
            return _function(instance);
        }
    }
}
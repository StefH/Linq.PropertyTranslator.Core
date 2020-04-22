using System;

namespace Linq.PropertyTranslator.Core.Examples.Interfaces
{
    public class Teacher : IPerson
    {
        private static readonly CompiledExpressionMap<Teacher, string> displayNameExpression
            = DefaultTranslationOf<Teacher>.Property(s => s.DisplayName).Is(s => s.Name);

        private static readonly CompiledExpressionMap<Teacher, double> sqrtExpression
            = DefaultTranslationOf<Teacher>.Property(s => s.Sqrt).Is(s => Math.Sqrt(s.Age + 7));

        public string DisplayName => displayNameExpression.Evaluate(this);

        public string Name { get; set; }

        public int Age { get; set; }

        public double Sqrt => sqrtExpression.Evaluate(this);
    }
}

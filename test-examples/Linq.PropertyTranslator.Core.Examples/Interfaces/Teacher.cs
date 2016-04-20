namespace Linq.PropertyTranslator.Core.Examples.Interfaces
{
    public class Teacher : IPerson
    {
        private static readonly CompiledExpressionMap<Teacher, string> displayNameExpression
            = DefaultTranslationOf<Teacher>.Property(s => s.DisplayName).Is(s => s.Name);

        public string DisplayName => displayNameExpression.Evaluate(this);

        public string Name { get; set; }
    }
}

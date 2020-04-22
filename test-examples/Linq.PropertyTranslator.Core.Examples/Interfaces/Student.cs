using Linq.PropertyTranslator.Core;
using Linq.PropertyTranslator.Core.Examples.Interfaces;

namespace Linq.PropertyTranslator.Examples.Interfaces
{
    public class Student : IPerson
    {
        private static readonly CompiledExpressionMap<Student, string> displayNameExpression
            = DefaultTranslationOf<Student>.Property(s => s.DisplayName).Is(s => s.Name + " (" + s.Id + ")");

        public string DisplayName
        {
            get { return displayNameExpression.Evaluate(this); }
        }

        public string Name { get; set; }

        public string Id { get; set; }

        public int Age { get; set; }

        public double Sqrt { get; set; } = 5;
    }
}

namespace Linq.PropertyTranslator.Core.Tests.Entities
{
    public class Employee
    {
        private static readonly CompiledExpressionMap<Employee, string> FullNameExpr =
            DefaultTranslationOf<Employee>.Property(e => e.FullName).Is(e => e.FirstName + " " + e.LastName);

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return FullNameExpr.Evaluate(this);
            }
        }
    }
}
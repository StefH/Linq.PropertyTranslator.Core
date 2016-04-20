using System.Collections.Generic;
using System.Linq;

namespace Linq.PropertyTranslator.Core.Examples.Interfaces
{
    public class InterfaceExample
    {
        public IEnumerable<T> Search<T>(IEnumerable<T> list, string name)
            where T : IPerson
        {
            return list.Where(p => p.DisplayName.Contains(name));
        }
    }
}